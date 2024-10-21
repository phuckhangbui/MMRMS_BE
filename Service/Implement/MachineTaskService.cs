using AutoMapper;
using Common;
using Common.Enum;
using DTOs.DeliveryTask;
using DTOs.MachineTask;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;

namespace Service.Implement
{
    public class MachineTaskService : IMachineTaskService
    {
        private readonly IMachineTaskRepository _machineTaskRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMachineCheckRequestRepository _MachineCheckRequestRepository;
        private readonly IComponentReplacementTicketRepository _ComponentReplacementTicketRepository;
        private readonly IDeliveryTaskRepository _DeliveryTaskRepository;
        private readonly IContractRepository _contractRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<MachineTaskHub> _machineTaskHub;
        private readonly IMapper _mapper;

        public MachineTaskService(IMachineTaskRepository MachineTaskRepository, IHubContext<MachineTaskHub> MachineTaskHub, INotificationService notificationService, IAccountRepository accountRepository, IMachineCheckRequestRepository MachineCheckRequestRepository, IDeliveryTaskRepository DeliveryTaskRepository, IMapper mapper, IContractRepository contractRepository, IComponentReplacementTicketRepository ComponentReplacementTicketRepository)
        {
            _machineTaskRepository = MachineTaskRepository;
            _machineTaskHub = MachineTaskHub;
            _notificationService = notificationService;
            _accountRepository = accountRepository;
            _MachineCheckRequestRepository = MachineCheckRequestRepository;
            _DeliveryTaskRepository = DeliveryTaskRepository;
            _mapper = mapper;
            _contractRepository = contractRepository;
            _ComponentReplacementTicketRepository = ComponentReplacementTicketRepository;
        }

        private async Task CheckCreateTaskCondition(int staffId, DateTime dateStart)
        {
            var staff = await _accountRepository.GetAccounById(staffId);

            if (staff == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            var staffTaskList = await _machineTaskRepository.GetTaskOfStaffInADay(staffId, dateStart)
                                        ?? Enumerable.Empty<MachineTaskDto>();
            var staffDeliveryTaskList = await _DeliveryTaskRepository.GetDeliveriesOfStaffInADay(staffId, dateStart)
                                        ?? Enumerable.Empty<DeliveryTaskDto>();

            int taskCounter = staffDeliveryTaskList.Count() + staffTaskList.Count();

            if (taskCounter >= GlobalConstant.MaxTaskLimitADayContract)
            {
                throw new ServiceException(MessageConstant.MachineTask.ReachMaxTaskLimit);
            }
        }

        public async Task CreateMachineTaskCheckMachine(int managerId, CreateMachineTaskCheckMachineDto createMachineTaskDto)
        {
            await CheckCreateTaskCondition(createMachineTaskDto.StaffId, createMachineTaskDto.DateStart);

            var requestDto = await _MachineCheckRequestRepository.GetMachineCheckRequest(createMachineTaskDto.RequestId);

            if (requestDto == null)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.RequestNotFound);
            }

            if (requestDto.Status != MachineCheckRequestStatusEnum.Processing.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotPossibleRequestStatus);
            }

            await _machineTaskRepository.CreateMachineTaskWithRequest(managerId, createMachineTaskDto);

            await _notificationService.SendNotificationToStaffWhenAssignTaskToCheckMachine(createMachineTaskDto.StaffId, requestDto.ContractAddress, createMachineTaskDto.DateStart);

            await _machineTaskHub.Clients.All.SendAsync("OnCreateMachineTask");
        }

        public async Task CreateMachineTaskProcessComponentReplacementTicket(int managerId, CreateMachineTaskProcessComponentReplacementTickett createMachineTaskDto)
        {
            await CheckCreateTaskCondition(createMachineTaskDto.StaffId, createMachineTaskDto.DateStart);

            var ticketDto = await _ComponentReplacementTicketRepository.GetTicket(createMachineTaskDto.ComponentReplacementTicketId);

            if (ticketDto == null)
            {
                throw new ServiceException(MessageConstant.MaintanningTicket.TicketNotFound);
            }

            if (ticketDto.Status != ComponentReplacementTicketStatusEnum.Paid.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotPossibleComponentReplacementTicketStatus);
            }


            //Todo

        }


        // currently comment out the controller, because task always have staffId when created
        public async Task DeleteMachineTask(int taskId)
        {
            var MachineTask = await _machineTaskRepository.GetMachineTask(taskId);

            if (MachineTask == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }

            if (MachineTask.StaffId != null)
            {
                throw new ServiceException(MessageConstant.MachineTask.CannotDeleted);
            }

            //need to check logic of the RequestResponse
            await _machineTaskRepository.Delete(taskId);

            await _machineTaskHub.Clients.All.SendAsync("OnDeleteMachineTask", taskId);
        }

        public async Task<MachineTaskDisplayDetail> GetMachineTaskDetail(int taskId)
        {
            var MachineTaskDetail = await _machineTaskRepository.GetMachineTaskDetail(taskId);

            if (MachineTaskDetail == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }
            var contract = await _contractRepository.GetContractDetailById(MachineTaskDetail.ContractId);

            MachineTaskDetail.CustomerId = contract.AccountOrder.AccountId;
            MachineTaskDetail.CustomerName = contract.AccountOrder.Name;
            MachineTaskDetail.CustomerPhone = contract.AccountOrder.Phone;

            var contractAddress = await _contractRepository.GetContractAddressById(MachineTaskDetail?.ContractId);

            MachineTaskDetail.Address = contractAddress;

            return MachineTaskDetail;
        }

        public async Task<IEnumerable<MachineTaskDto>> GetMachineTasks()
        {
            var list = await _machineTaskRepository.GetMachineTasks();

            return list;
        }

        public async Task<IEnumerable<MachineTaskDto>> GetMachineTasks(int staffId)
        {
            var list = await _machineTaskRepository.GetMachineTaskByStaff(staffId);

            return list;
        }

        public async Task UpdateMachineTaskStatus(int MachineTaskId, string status, int accountId)
        {
            var MachineTask = await _machineTaskRepository.GetMachineTask(MachineTaskId);

            var account = await _accountRepository.GetAccounById(accountId);

            if (MachineTask == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(MachineTaskStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.MachineTask.StatusNotAvailable);
            }
            //logic here


            await _machineTaskRepository.UpdateTaskStatus(MachineTaskId, status, accountId);

            //TODO:KHANG
            //if (account.RoleId == (int)AccountRoleEnum.Staff)
            //{
            //    await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated((int)MachineTask.ManagerId, MachineTask.TaskTitle, status);
            //}

            if (account.RoleId == (int)AccountRoleEnum.Manager)
            {
                await _notificationService.SendNotificationToStaffWhenTaskStatusUpdated((int)MachineTask.StaffId, MachineTask.TaskTitle, status);
            }

            await _machineTaskHub.Clients.All.SendAsync("OnUpdateMachineTaskStatus", MachineTaskId);
        }
    }
}
