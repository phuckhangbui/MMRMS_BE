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
        private readonly IMachineCheckRequestRepository _machineCheckRequestRepository;
        private readonly IComponentReplacementTicketRepository _componentReplacementTicketRepository;
        private readonly IRequestResponseRepository _requestResponseRepository;
        private readonly IDeliveryTaskRepository _DeliveryTaskRepository;
        private readonly IContractRepository _contractRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<MachineTaskHub> _machineTaskHub;
        private readonly IMapper _mapper;

        public MachineTaskService(IMachineTaskRepository MachineTaskRepository, IHubContext<MachineTaskHub> MachineTaskHub, INotificationService notificationService, IAccountRepository accountRepository, IMachineCheckRequestRepository MachineCheckRequestRepository, IDeliveryTaskRepository DeliveryTaskRepository, IMapper mapper, IContractRepository contractRepository, IComponentReplacementTicketRepository ComponentReplacementTicketRepository, IRequestResponseRepository requestResponseRepository)
        {
            _machineTaskRepository = MachineTaskRepository;
            _machineTaskHub = MachineTaskHub;
            _notificationService = notificationService;
            _accountRepository = accountRepository;
            _machineCheckRequestRepository = MachineCheckRequestRepository;
            _DeliveryTaskRepository = DeliveryTaskRepository;
            _mapper = mapper;
            _contractRepository = contractRepository;
            _componentReplacementTicketRepository = ComponentReplacementTicketRepository;
            _requestResponseRepository = requestResponseRepository;
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

            var requestDto = await _machineCheckRequestRepository.GetMachineCheckRequest(createMachineTaskDto.RequestId);

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

        public async Task CreateMachineTaskProcessComponentReplacementTicket(int managerId, CreateMachineTaskProcessComponentReplacementTicket createMachineTaskDto)
        {
            await CheckCreateTaskCondition(createMachineTaskDto.StaffId, createMachineTaskDto.DateStart);

            var ticketDto = await _componentReplacementTicketRepository.GetTicket(createMachineTaskDto.ComponentReplacementTicketId);

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
            var machineTask = await _machineTaskRepository.GetMachineTask(taskId);

            if (machineTask == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }

            if (machineTask.StaffId != null)
            {
                throw new ServiceException(MessageConstant.MachineTask.CannotDeleted);
            }

            //need to check logic of the RequestResponse
            await _machineTaskRepository.Delete(taskId);

            await _machineTaskHub.Clients.All.SendAsync("OnDeleteMachineTask", taskId);
        }

        public async Task<MachineTaskDisplayDetail> GetMachineTaskDetail(int taskId)
        {
            var machineTaskDetail = await _machineTaskRepository.GetMachineTaskDetail(taskId);

            if (machineTaskDetail == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }
            var contract = await _contractRepository.GetContractDetailById(machineTaskDetail.ContractId);

            machineTaskDetail.CustomerId = contract.AccountOrder.AccountId;
            machineTaskDetail.CustomerName = contract.AccountOrder.Name;
            machineTaskDetail.CustomerPhone = contract.AccountOrder.Phone;

            var contractAddress = await _contractRepository.GetContractAddressById(machineTaskDetail?.ContractId);

            machineTaskDetail.Address = contractAddress;

            return machineTaskDetail;
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

        //will remove this 

        public async Task StaffCheckMachineSuccess(int taskId, int staffId, string? confirmationPictureUrl)
        {
            var machineTask = await _machineTaskRepository.GetMachineTask(taskId);

            if (machineTask == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }

            if (machineTask.Type != MachineTaskTypeEnum.MachineryCheck.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.NotCorrectTaskType);
            }

            if (machineTask.Status != MachineTaskStatusEnum.Assigned.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.StatusCannotSet);
            }

            //Todo logic here
            await _machineTaskRepository.UpdateTaskStatus(taskId, MachineTaskStatusEnum.Completed.ToString(), staffId, confirmationPictureUrl);

            //await _requestResponseRepository.CreateResponeWhenCheckMachineTaskSuccess((int)machineTask.RequestResponseId);

            await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated((int)machineTask.ManagerId, machineTask.TaskTitle, MachineTaskStatusEnum.Completed.ToString());
        }

        public async Task StaffReplaceComponentSuccess(int taskId, int staffId, string? confirmationPictureUrl)
        {
            var machineTask = await _machineTaskRepository.GetMachineTask(taskId);

            if (machineTask == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }

            if (machineTask.Type != MachineTaskTypeEnum.ComponentReplacement.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.NotCorrectTaskType);
            }

            if (machineTask.Status != MachineTaskStatusEnum.Assigned.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.StatusCannotSet);
            }

            //Todo logic here

            await _machineTaskRepository.UpdateTaskStatus(taskId, MachineTaskStatusEnum.Completed.ToString(), staffId, confirmationPictureUrl);

            await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated((int)machineTask.ManagerId, machineTask.TaskTitle, MachineTaskStatusEnum.Completed.ToString());
        }
    }
}
