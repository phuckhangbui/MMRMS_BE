using AutoMapper;
using Common;
using Common.Enum;
using DTOs.Delivery;
using DTOs.MachineTask;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
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

            if (staff.Status != AccountStatusEnum.Active.ToString())
            {
                throw new ServiceException(MessageConstant.Account.AccountChosenLocked);
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
                    var task = await this.CreateMachineTaskWithRequest(managerId, createMachineTaskDto);

                    await _machineCheckRequestService.UpdateRequestStatus(createMachineTaskDto.RequestId, MachineCheckRequestStatusEnum.Assigned.ToString(), task.MachineTaskId);

                    await _notificationService.SendNotificationToStaffWhenAssignTaskToCheckMachine(createMachineTaskDto.StaffId,
                                                                                                   requestDto.ContractAddress,
                                                                                                   parsedDate,
                                                                                                   task?.MachineTaskId.ToString() ?? null);

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
                throw new ServiceException(MessageConstant.MachineTask.TaskTerminationNotPossibleContractStatus);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var task = await this.CreateMachineTaskContractTermination(managerId, createMachineTaskDto);

                    //update contract status
                    await _contractRepository.UpdateContractStatus(createMachineTaskDto.ContractId, ContractStatusEnum.InspectionInProgress.ToString());

                    var contractAddress = await _contractRepository.GetContractAddressById(createMachineTaskDto.ContractId);

                    await _notificationService.SendNotificationToStaffWhenAssignTaskToCheckMachine(createMachineTaskDto.StaffId, contractAddress, parsedDate,
                                                                                                   task?.MachineTaskId.ToString() ?? null);

                    await _machineTaskHub.Clients.All.SendAsync("OnCreateMachineTask");

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.MachineTask.CreateFail);
                }
            }

        }

        public async Task CreateMachineTaskCheckMachineDeliveryFail(int managerId, CreateMachineTaskDeliveryFailDto createMachineTaskDto)
        {
            if (!DateTime.TryParseExact(createMachineTaskDto.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new ServiceException("Format ngày không đúng, xin hãy dùng 'yyyy-MM-dd'.");
            }

            await CheckCreateTaskCondition(createMachineTaskDto.StaffId, parsedDate);

            var contractDeliveryDto = await _contractRepository.GetContractDelivery(createMachineTaskDto.ContractDeliveryId);



            if (contractDeliveryDto == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractDeliveryNotFound);
            }

            if (contractDeliveryDto.Status != ContractDeliveryStatusEnum.Fail.ToString())
            {
                throw new ServiceException(MessageConstant.MachineTask.TaskCheckDeliveryFailNotPossibleContractStatus);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var task = await this.CreateMachineTaskCheckMachineWhenDeliveryFail(managerId, createMachineTaskDto);
                    //update contract delivery status
                    await _contractRepository.UpdateContractDeliveryStatus(createMachineTaskDto.ContractDeliveryId, ContractDeliveryStatusEnum.ProcessedAfterFailure.ToString());
                    //update contract status
                    await _contractRepository.UpdateContractStatus(contractDeliveryDto.ContractId, ContractStatusEnum.InspectionInProgress.ToString());

                    await _notificationService.SendNotificationToStaffWhenAssignTaskToCheckMachineInStorage(createMachineTaskDto.StaffId, task, parsedDate,
                                                                                                            task?.MachineTaskId.ToString() ?? null);

                    await _machineTaskHub.Clients.All.SendAsync("OnCreateMachineTask");

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.MachineTask.CreateFail);
                }
            }
        }

        private async Task<MachineTaskDto> CreateMachineTaskInternal(int managerId,
                                                            CreateMachineTaskDtoBase createMachineTaskDto,
                                                            string taskType,
                                                            string logAction)
        {
            // Parse DateStart
            if (!DateTime.TryParseExact(createMachineTaskDto.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new Exception("Format ngày không đúng, xin hãy dùng 'yyyy-MM-dd'.");
            }

            var staffAccount = await _accountRepository.GetAccounById(createMachineTaskDto.StaffId);
            var now = DateTime.Now;

            var taskDto = new MachineTaskDto
            {
                TaskTitle = createMachineTaskDto.TaskTitle,
                Content = createMachineTaskDto.TaskContent,
                StaffId = createMachineTaskDto.StaffId,
                ManagerId = managerId,
                Type = taskType,
                DateCreate = now,
                DateStart = parsedDate,
                Status = MachineTaskEnum.Created.ToString(),
                Note = createMachineTaskDto.Note
            };

            if (createMachineTaskDto is CreateMachineTaskCheckRequestDto checkMachineDto)
            {
                var request = await _machineCheckRequestRepository.GetMachineCheckRequest(checkMachineDto.RequestId);
                taskDto.MachineCheckRequestId = checkMachineDto.RequestId;
                taskDto.ContractId = request.ContractId;
            }
            else if (createMachineTaskDto is CreateMachineTaskContractTerminationDto terminationDto)
            {
                taskDto.ContractId = terminationDto.ContractId;
            }
            else if (createMachineTaskDto is CreateMachineTaskDeliveryFailDto shipFailDto)
            {
                var contractDeliveryDto = await _contractRepository.GetContractDelivery(shipFailDto.ContractDeliveryId);

                taskDto.ContractId = contractDeliveryDto.ContractId;
            }

            var taskLogDto = new MachineTaskLogDto
            {
                Action = logAction.Replace("{staffName}", staffAccount.Name),
                DateCreate = now,
                AccountTriggerId = managerId
            };

            taskDto = await _machineTaskRepository.CreateMachineTaskWithLog(taskDto, taskLogDto);

            return taskDto;
        }

        private async Task<MachineTaskDto> CreateMachineTaskContractTermination(int managerId, CreateMachineTaskContractTerminationDto createMachineTaskDto)
        {
            return await CreateMachineTaskInternal(managerId, createMachineTaskDto,
                                   MachineTaskTypeEnum.ContractTerminationCheck.ToString(),
                                   "Tạo và giao công việc kiêm tra máy chấm dứt hợp đồng cho {staffName}");
        }

        private async Task<MachineTaskDto> CreateMachineTaskWithRequest(int managerId, CreateMachineTaskCheckRequestDto createMachineTaskDto)
        {

            return await CreateMachineTaskInternal(managerId, createMachineTaskDto,
                                        MachineTaskTypeEnum.MachineryCheckRequest.ToString(),
                                        "Tạo và giao công việc kiểm tra máy cho {staffName}");
        }

        private async Task<MachineTaskDto> CreateMachineTaskCheckMachineWhenDeliveryFail(int managerId, CreateMachineTaskDeliveryFailDto createMachineTaskDto)
        {
            return await CreateMachineTaskInternal(managerId, createMachineTaskDto,
                                  MachineTaskTypeEnum.DeliveryFailCheckRequest.ToString(),
                                  "Tạo và giao công việc kiêm tra máy khi khách từ chối nhận máy lúc giao cho {staffName}");
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
                isAllTicketCompleted = machineTaskDetail.ComponentReplacementTicketCreateFromTaskList
                    .All(componentReplacementTicket =>
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

            if (machineTaskDetail.Type == MachineTaskTypeEnum.DeliveryFailCheckRequest.ToString())
            {
                var componentList = await _machineSerialNumberRepository.GetMachineComponent(machineTaskDetail.SerialNumber);

                var isMachineMaintenance = false;

                if (!componentList.IsNullOrEmpty())
                {
                    isMachineMaintenance = componentList.Any(c => c.Status == MachineSerialNumberComponentStatusEnum.Broken.ToString());
                }

                if (isMachineMaintenance)
                {
                    throw new ServiceException(MessageConstant.MachineTask.TaskShipFailCannotCompleteDueToMachineStillHaveBrokenComponent);
                }
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

                    if (machineTaskDetail.Type == MachineTaskTypeEnum.ContractTerminationCheck.ToString())
                    {
                        await _contractRepository.UpdateContractStatus(machineTaskDetail.ContractId,
                                                            ContractStatusEnum.AwaitingRefundInvoice.ToString());
                    }

                    if (machineTaskDetail.Type == MachineTaskTypeEnum.DeliveryFailCheckRequest.ToString())
                    {

                        await _contractRepository.UpdateContractStatus(machineTaskDetail.ContractId,
                                                            ContractStatusEnum.AwaitingShippingAfterCheck.ToString());
                    }

                    await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated((int)machineTaskDetail.ManagerId,
                                                                                                machineTaskDetail.TaskTitle,
                                                                                                EnumExtensions.ToVietnamese(MachineTaskEnum.Completed),
                                                                                                machineTaskDetail?.MachineTaskId.ToString() ?? null);
                    await _machineTaskHub.Clients.All.SendAsync("OnUpdateMachineTaskStatus", machineTaskDetail.MachineTaskId);

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
