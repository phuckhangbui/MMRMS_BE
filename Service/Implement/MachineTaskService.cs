using AutoMapper;
using Common;
using Common.Enum;
using DTOs.Delivery;
using DTOs.MachineTask;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;
using System.Globalization;
using System.Transactions;

namespace Service.Implement
{
    public class MachineTaskService : IMachineTaskService
    {
        private readonly IMachineTaskRepository _machineTaskRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMachineCheckRequestRepository _machineCheckRequestRepository;
        private readonly IMachineCheckRequestService _machineCheckRequestService;
        private readonly IComponentReplacementTicketRepository _componentReplacementTicketRepository;
        private readonly IDeliveryTaskRepository _DeliveryTaskRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<MachineTaskHub> _machineTaskHub;
        private readonly IMapper _mapper;

        public MachineTaskService(IMachineTaskRepository MachineTaskRepository, IHubContext<MachineTaskHub> MachineTaskHub, INotificationService notificationService, IAccountRepository accountRepository, IMachineCheckRequestRepository machineCheckRequestRepository, IDeliveryTaskRepository DeliveryTaskRepository, IMapper mapper, IContractRepository contractRepository, IComponentReplacementTicketRepository ComponentReplacementTicketRepository, IMachineCheckRequestService machineCheckRequestService, IMachineSerialNumberRepository machineSerialNumberRepository)
        {
            _machineTaskRepository = MachineTaskRepository;
            _machineTaskHub = MachineTaskHub;
            _notificationService = notificationService;
            _accountRepository = accountRepository;
            _DeliveryTaskRepository = DeliveryTaskRepository;
            _mapper = mapper;
            _contractRepository = contractRepository;
            _componentReplacementTicketRepository = ComponentReplacementTicketRepository;
            _machineCheckRequestRepository = machineCheckRequestRepository;
            _machineCheckRequestService = machineCheckRequestService;
            _machineSerialNumberRepository = machineSerialNumberRepository;
        }

        private async Task CheckCreateTaskCondition(int staffId, DateTime dateStart)
        {
            var staff = await _accountRepository.GetAccounById(staffId);

            if (staff == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            if (staff.RoleId != (int)AccountRoleEnum.TechnicalStaff)
            {
                throw new ServiceException(MessageConstant.Account.AccountRoleIsNotSuitableToAssignForThisTask);
            }

            var staffTaskList = await _machineTaskRepository.GetTaskOfStaffInADay(staffId, dateStart)
                                        ?? Enumerable.Empty<MachineTaskDto>();
            var staffDeliveryTaskList = await _DeliveryTaskRepository.GetDeliveriesOfStaffInADay(staffId, dateStart)
                                        ?? Enumerable.Empty<DeliveryTaskDto>();

            int taskCounter = staffDeliveryTaskList.Count() + staffTaskList.Count();

            if (taskCounter >= GlobalConstant.MaxTaskLimitADay)
            {
                throw new ServiceException(MessageConstant.MachineTask.ReachMaxTaskLimit);
            }
        }

        public async Task CreateMachineTaskCheckMachine(int managerId, CreateMachineTaskCheckRequestDto createMachineTaskDto)
        {
            if (!DateTime.TryParseExact(createMachineTaskDto.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new ServiceException("Format ngày không đúng, xin hãy dùng 'yyyy-MM-dd'.");
            }

            await CheckCreateTaskCondition(createMachineTaskDto.StaffId, parsedDate);

            var requestDto = await _machineCheckRequestRepository.GetMachineCheckRequest(createMachineTaskDto.RequestId);

            if (requestDto == null)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.RequestNotFound);
            }

            if (requestDto.Status != MachineCheckRequestStatusEnum.New.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotPossibleRequestStatus);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var task = await _machineTaskRepository.CreateMachineTaskWithRequest(managerId, createMachineTaskDto);

                    await _machineCheckRequestService.UpdateRequestStatus(createMachineTaskDto.RequestId, MachineCheckRequestStatusEnum.Assigned.ToString(), task.MachineTaskId);

                    await _notificationService.SendNotificationToStaffWhenAssignTaskToCheckMachine(createMachineTaskDto.StaffId, requestDto.ContractAddress, parsedDate);

                    await _machineTaskHub.Clients.All.SendAsync("OnCreateMachineTask");

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.CreateFail);
                }
            }


        }

        public async Task CreateMachineTaskCheckMachineContractTermination(int managerId, CreateMachineTaskContractTerminationDto createMachineTaskDto)
        {
            if (!DateTime.TryParseExact(createMachineTaskDto.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new ServiceException("Format ngày không đúng, xin hãy dùng 'yyyy-MM-dd'.");
            }

            await CheckCreateTaskCondition(createMachineTaskDto.StaffId, parsedDate);

            var contractDto = await _contractRepository.GetContractById(createMachineTaskDto.ContractId);

            if (contractDto == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }


            if (contractDto.Status != ContractStatusEnum.InspectionPending.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotPossibleContractStatus);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var task = await _machineTaskRepository.CreateMachineTaskContractTermination(managerId, createMachineTaskDto);

                    //update contract status
                    await _contractRepository.UpdateContractStatus(createMachineTaskDto.ContractId, ContractStatusEnum.InspectionInProgress.ToString());

                    var contractAddress = await _contractRepository.GetContractAddressById(createMachineTaskDto.ContractId);

                    await _notificationService.SendNotificationToStaffWhenAssignTaskToCheckMachine(createMachineTaskDto.StaffId, contractAddress, parsedDate);

                    await _machineTaskHub.Clients.All.SendAsync("OnCreateMachineTask");

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.MachineTask.CreateFail);
                }
            }

        }

        public async Task CreateMachineTaskProcessComponentReplacementTicket(int managerId, CreateMachineTaskProcessComponentReplacementTicket createMachineTaskDto)
        {
            await CheckCreateTaskCondition(createMachineTaskDto.StaffId, createMachineTaskDto.DateStart);

            var ticketDto = await _componentReplacementTicketRepository.GetTicket(createMachineTaskDto.ComponentReplacementTicketId);

            if (ticketDto == null)
            {
                throw new ServiceException(MessageConstant.ComponentReplacementTicket.TicketNotFound);
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

            var requestDetail = await _machineCheckRequestRepository.GetMachineCheckRequestDetail(machineTaskDetail.MachineCheckRequestId);

            if (requestDetail != null)
            {
                machineTaskDetail.MachineCheckRequest = requestDetail;
            }

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

        public async Task StaffCheckMachineSuccess(int taskId, int staffId, string? confirmationPictureUrl)
        {
            var machineTaskDetail = await _machineTaskRepository.GetMachineTaskDetail(taskId);

            if (machineTaskDetail == null)
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
            }

            if (machineTaskDetail.StaffId != staffId)
            {
                throw new ServiceException(MessageConstant.MachineTask.IncorrectStaffIdToUpdate);
            }
            var isAllTicketCompleted = true;


            if (machineTaskDetail.ComponentReplacementTicketCreateFromTaskList != null &&
                machineTaskDetail.ComponentReplacementTicketCreateFromTaskList.Count() > 0)
            {
                isAllTicketCompleted = machineTaskDetail.ComponentReplacementTicketCreateFromTaskList.All(componentReplacementTicket =>
                                                           componentReplacementTicket.Status == ComponentReplacementTicketStatusEnum.Completed.ToString() ||
                                                           componentReplacementTicket.Status == ComponentReplacementTicketStatusEnum.Canceled.ToString());

                if (!isAllTicketCompleted &&
                    machineTaskDetail.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString())
                {
                    throw new ServiceException(MessageConstant.MachineTask.TaskCannotCompleteDueToTicketListUnfulfill);
                }
            }

            if (machineTaskDetail.Status == MachineTaskEnum.Canceled.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.StatusCannotSet);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _machineTaskRepository.UpdateTaskStatus(taskId, MachineTaskEnum.Completed.ToString(), staffId, confirmationPictureUrl);

                    if (machineTaskDetail.MachineCheckRequestId != null && machineTaskDetail.Type == MachineTaskTypeEnum.MachineryCheckRequest.ToString())
                    {
                        await _machineCheckRequestService.UpdateRequestStatus(machineTaskDetail.MachineCheckRequestId
                                                             , MachineCheckRequestStatusEnum.Completed.ToString(), null);
                    }

                    //logic when check success in case of terminate contract check
                    if (machineTaskDetail.Type == MachineTaskTypeEnum.ContractTerminationCheck.ToString())
                    {

                        await _contractRepository.UpdateContractStatus(machineTaskDetail.ContractId,
                                                            ContractStatusEnum.AwaitingRefundInvoice.ToString());

                        if (machineTaskDetail.ComponentReplacementTicketCreateFromTaskList != null
                            && machineTaskDetail.ComponentReplacementTicketCreateFromTaskList.Count() > 0)
                        {
                            await _machineSerialNumberRepository.UpdateStatus(machineTaskDetail.SerialNumber,
                                                             MachineSerialNumberStatusEnum.Maintenance.ToString(),      staffId);
                        }
                    }

                    await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated((int)machineTaskDetail.ManagerId, machineTaskDetail.TaskTitle, EnumExtensions.ToVietnamese(MachineTaskEnum.Completed));

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.CreateFail);
                }
            }

        }



        //public async Task StaffReplaceComponentSuccess(int taskId, int staffId, string? confirmationPictureUrl)
        //{
        //    var machineTask = await _machineTaskRepository.GetMachineTask(taskId);

        //    if (machineTask == null)
        //    {
        //        throw new ServiceException(MessageConstant.MachineTask.TaskNotFound);
        //    }

        //    if (machineTask.Type != MachineTaskTypeEnum.ComponentReplacement.ToString())
        //    {
        //        throw new ServiceException(MessageConstant.MachineTask.NotCorrectTaskType);
        //    }

        //    if (machineTask.Status != MachineTaskStatusEnum.Created.ToString())
        //    {
        //        throw new ServiceException(MessageConstant.MachineTask.StatusCannotSet);
        //    }

        //    //Todo logic here

        //    await _machineTaskRepository.UpdateTaskStatus(taskId, MachineTaskStatusEnum.Completed.ToString(), staffId, confirmationPictureUrl);

        //    await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated((int)machineTask.ManagerId, machineTask.TaskTitle, EnumExtensions.ToVietnamese(MachineTaskStatusEnum.Completed));
        //}
    }
}
