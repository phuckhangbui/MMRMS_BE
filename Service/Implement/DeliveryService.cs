using Common;
using Common.Enum;
using DTOs.Contract;
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
    public class DeliveryService : IDeliverService
    {

        private readonly IDeliveryTaskRepository _deliveryTaskRepository;
        private readonly IMachineTaskRepository _machineTaskRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRentingRequestRepository _rentingRequestRepository;
        private readonly INotificationService _notificationService;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IHubContext<DeliveryTaskHub> _deliveryTaskHub;
        private readonly IBackground _background;

        public DeliveryService(IDeliveryTaskRepository DeliveryTaskRepository, IMachineTaskRepository MachineTaskRepository, IAccountRepository accountRepository, IHubContext<DeliveryTaskHub> DeliveryTaskHub, INotificationService notificationService, IContractRepository contractRepository, IRentingRequestRepository rentingRequestRepository, IMachineSerialNumberRepository machineSerialNumberRepository, IBackground background)
        {
            _deliveryTaskRepository = DeliveryTaskRepository;
            _machineTaskRepository = MachineTaskRepository;
            _accountRepository = accountRepository;
            _deliveryTaskHub = DeliveryTaskHub;
            _notificationService = notificationService;
            _contractRepository = contractRepository;
            _rentingRequestRepository = rentingRequestRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _background = background;
        }

        public async Task CreateDeliveryTask(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto)
        {
            var accountDto = await _accountRepository.GetAccounById(createDeliveryTaskDto.StaffId);

            if (!DateTime.TryParseExact(createDeliveryTaskDto.DateShip, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new ServiceException("Format ngày không đúng, xin hãy dùng 'yyyy-MM-dd'.");
            }


            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            if (accountDto.RoleId != (int)AccountRoleEnum.TechnicalStaff)
            {
                throw new ServiceException(MessageConstant.Account.AccountRoleIsNotSuitableToAssignForThisTask);
            }

            if (accountDto.Status != AccountStatusEnum.Active.ToString())
            {
                throw new ServiceException(MessageConstant.Account.AccountChosenLocked);
            }

            var taskList = await _machineTaskRepository.GetTaskOfStaffInADay(createDeliveryTaskDto.StaffId, parsedDate)
                                        ?? Enumerable.Empty<MachineTaskDto>();

            var deliveryTaskList = await _deliveryTaskRepository.GetDeliveriesOfStaffInADay(createDeliveryTaskDto.StaffId, parsedDate)
                                        ?? Enumerable.Empty<DeliveryTaskDto>();

            int taskCounter = taskList.Count() + deliveryTaskList.Count();


            if (taskCounter >= GlobalConstant.MaxTaskLimitADay)
            {
                throw new ServiceException(MessageConstant.MachineTask.ReachMaxTaskLimit);
            }

            string rentingRequestId = null;
            var contractList = new List<ContractDto>();

            foreach (string contractId in createDeliveryTaskDto.ContractIdList)
            {
                var contract = await _contractRepository.GetContractById(contractId);

                if (contract == null)
                {
                    throw new ServiceException(MessageConstant.Contract.ContractNotFound);
                }


                if (contract.Status != ContractStatusEnum.Signed.ToString()
                    && contract.Status != ContractStatusEnum.AwaitingShippingAfterCheck.ToString()
                    && contract.Status != ContractStatusEnum.ShipFail.ToString())
                {
                    throw new ServiceException(MessageConstant.Contract.ContractNotValidToDelivery + contractId);
                }

                var contractDeliveryList = await _contractRepository.GetContractDeliveryBaseOnContractId(contractId);

                if (contractDeliveryList.Any(c => c.Status == ContractDeliveryStatusEnum.Pending.ToString()
                                               || c.Status == ContractDeliveryStatusEnum.Success.ToString()))
                {
                    throw new ServiceException(MessageConstant.Contract.ContractNotValidToDeliveryOldContractDeliveryStillActive + contractId);
                }

                if (rentingRequestId.IsNullOrEmpty())
                {
                    rentingRequestId = contract.RentingRequestId;
                }

                if (rentingRequestId != contract.RentingRequestId)
                {
                    throw new ServiceException(MessageConstant.DeliveryTask.ContractAreNotInTheSameRequest);
                }

                contractList.Add(contract);
            }

            var deliveryDto = await _deliveryTaskRepository.CreateDeliveryTaskToStaff(managerId, createDeliveryTaskDto);

            foreach (var contractId in createDeliveryTaskDto.ContractIdList)
            {
                await _contractRepository.UpdateContractStatus(contractId, ContractStatusEnum.Shipping.ToString());

                //need to send noti to customer ?
            }

            var contractAddress = await _contractRepository.GetContractAddressById(createDeliveryTaskDto.ContractIdList.FirstOrDefault());

            await _notificationService.SendNotificationToStaffWhenAssignDeliveryTask((int)deliveryDto.StaffId, contractAddress,
                                                                                (DateTime)deliveryDto.DateShip,
                                                                                deliveryDto?.DeliveryTaskId.ToString() ?? null);

            await _deliveryTaskHub.Clients.All.SendAsync("OnCreateDeliveryTaskToStaff", deliveryDto.DeliveryTaskId);
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveries()
        {
            return await _deliveryTaskRepository.GetDeliveries();
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveries(int staffId)
        {
            return await _deliveryTaskRepository.GetDeliveriesForStaff(staffId);
        }

        public async Task<DeliveryTaskDetailDto> GetDeliveryDetail(int deliveryTaskId)
        {
            try
            {

                return await _deliveryTaskRepository.GetDeliveryTaskDetail(deliveryTaskId);
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }
        }

        public async Task StaffCompleteDelivery(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto, int accountId)
        {
            var deliveryDetail = await _deliveryTaskRepository.GetDeliveryTaskDetail(staffUpdateDeliveryTaskDto.DeliveryTaskId);

            if (deliveryDetail == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            if (accountId != deliveryDetail.DeliveryTask.StaffId)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.YouCannotChangeThisDelivery);
            }

            if (deliveryDetail.DeliveryTask.Status != DeliveryTaskStatusEnum.Delivering.ToString())
            {
                throw new ServiceException(MessageConstant.DeliveryTask.StatusCannotSet);
            }

            if (deliveryDetail.ContractDeliveries.Count() != staffUpdateDeliveryTaskDto.ContractDeliveries.Count()
                || !deliveryDetail.ContractDeliveries.Select(d => d.ContractDeliveryId)
                    .OrderBy(id => id)
                    .SequenceEqual(staffUpdateDeliveryTaskDto.ContractDeliveries.Select(d => d.ContractDeliveryId).OrderBy(id => id)))
            {
                throw new ServiceException(MessageConstant.DeliveryTask.InvalidContractDeliveryList);
            }

            if (staffUpdateDeliveryTaskDto.ContractDeliveries.All(c => !c.IsSuccess))
            {
                throw new ServiceException(MessageConstant.DeliveryTask.AllContractDeliveryFailPleaseCallAllFailAPI);
            }


            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (staffUpdateDeliveryTaskDto.ContractDeliveries.All(c => c.IsSuccess))
                    {
                        await this.CompleteAllDeliveryInTask(staffUpdateDeliveryTaskDto, deliveryDetail, accountId);
                    }
                    else
                    {
                        await this.CompletePartialDeliveryInTask(staffUpdateDeliveryTaskDto, deliveryDetail, accountId);
                    }
                    scope.Complete();

                    await _deliveryTaskHub.Clients.All.SendAsync("OnUpdateDeliveryTaskStatus", deliveryDetail.DeliveryTask.DeliveryTaskId);
                }
                catch (Exception ex)
                {
                    throw new ServiceException(ex.ToString());
                }
            }
        }

        private async Task CompletePartialDeliveryInTask(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto, DeliveryTaskDetailDto deliveryDetail, int accountId)
        {
            await _deliveryTaskRepository.MarkDeliveryTaskAsFail(staffUpdateDeliveryTaskDto);

            foreach (var contractDeliveryDto in staffUpdateDeliveryTaskDto.ContractDeliveries)
            {
                var contractDelivery = deliveryDetail.ContractDeliveries
                    .FirstOrDefault(cd => cd.ContractDeliveryId == contractDeliveryDto.ContractDeliveryId);

                if (contractDelivery != null)
                {
                    var contractId = contractDelivery.ContractId;

                    if (contractDeliveryDto.IsSuccess)
                    {
                        await _contractRepository.UpdateContractStatus(contractId, ContractStatusEnum.Renting.ToString());
                        await _machineSerialNumberRepository.UpdateStatus(contractDelivery.SerialNumber, MachineSerialNumberStatusEnum.Renting.ToString(), accountId);

                        //Schedule Background Job
                        var contractDto = await _contractRepository.GetContractById(contractId);
                        TimeSpan timeUntilEnd = (TimeSpan)(contractDto.DateEnd - DateTime.Now);
                        _background.CompleteContractOnTimeJob(contractId, timeUntilEnd);
                    }
                    else
                    {
                        await _contractRepository.UpdateContractStatus(contractId, ContractStatusEnum.ShipFail.ToString());
                    }

                }
            }

            await _notificationService.SendNotificationToManagerWhenDeliveryTaskStatusUpdated(
                (int)deliveryDetail.DeliveryTask.ManagerId,
                deliveryDetail.DeliveryTask.ContractAddress,
                EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Fail),
                deliveryDetail?.DeliveryTask?.DeliveryTaskId.ToString() ?? null
            );

            await _notificationService.SendNotificationToCustomerWhenDeliveryTaskStatusUpdated(
              (int)deliveryDetail.DeliveryTask.CustomerId,
              deliveryDetail.DeliveryTask.ContractAddress,
              EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Fail),
              deliveryDetail?.DeliveryTask?.DeliveryTaskId.ToString() ?? null
          );
        }




        private async Task CompleteAllDeliveryInTask(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto, DeliveryTaskDetailDto deliveryDetail, int accountId)
        {
            await _deliveryTaskRepository.CompleteFullyAllDeliveryTask(staffUpdateDeliveryTaskDto);

            string contractId = "";
            foreach (var contractDelivery in deliveryDetail.ContractDeliveries)
            {
                contractId = contractDelivery.ContractId;

                await _contractRepository.UpdateContractStatus(contractId, ContractStatusEnum.Renting.ToString());

                await _machineSerialNumberRepository.UpdateStatus(contractDelivery.SerialNumber, MachineSerialNumberStatusEnum.Renting.ToString(), accountId);

                //Schedule Background Job
                var contractDto = await _contractRepository.GetContractById(contractId);
                TimeSpan timeUntilEnd = (TimeSpan)(contractDto.DateEnd - DateTime.Now);
                _background.CompleteContractOnTimeJob(contractId, timeUntilEnd);
            }

            var contract = await _contractRepository.GetContractById(contractId);
            if (contract != null)
            {
                var rentingRequest = await _rentingRequestRepository.GetRentingRequestDetailById(contract.RentingRequestId);

                if (rentingRequest.Contracts != null && rentingRequest.Contracts.All(c => c.Status == ContractStatusEnum.Renting.ToString()))
                {
                    await _rentingRequestRepository.UpdateRentingRequestStatus(rentingRequest.RentingRequestId, RentingRequestStatusEnum.Shipped.ToString());

                    //need to send noti to customer ?
                }
            }

            await _notificationService.SendNotificationToManagerWhenDeliveryTaskStatusUpdated((int)deliveryDetail.DeliveryTask.ManagerId,
                                                                                               deliveryDetail.DeliveryTask.ContractAddress,
                                                                                               EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Completed),
                                                                                               deliveryDetail?.DeliveryTask?.DeliveryTaskId.ToString() ?? null);
            await _notificationService.SendNotificationToCustomerWhenDeliveryTaskStatusUpdated(
             (int)deliveryDetail.DeliveryTask.CustomerId,
             deliveryDetail.DeliveryTask.ContractAddress,
             EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Completed),
             deliveryDetail?.DeliveryTask?.DeliveryTaskId.ToString() ?? null
         );
        }

        public async Task StaffFailDelivery(StaffFailDeliveryTaskDto staffFailDeliveryTask, int accountId)
        {
            var deliveryDetail = await _deliveryTaskRepository.GetDeliveryTaskDetail(staffFailDeliveryTask.DeliveryTaskId);

            if (deliveryDetail == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            if (accountId != deliveryDetail.DeliveryTask.StaffId)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.YouCannotChangeThisDelivery);
            }

            if (deliveryDetail.DeliveryTask.Status != DeliveryTaskStatusEnum.Delivering.ToString())
            {
                throw new ServiceException(MessageConstant.DeliveryTask.StatusCannotSet);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _deliveryTaskRepository.FailDeliveryTask(staffFailDeliveryTask);
                    foreach (var contractDelivery in deliveryDetail.ContractDeliveries)
                    {
                        var contractId = contractDelivery.ContractId;

                        await _contractRepository.UpdateContractStatus(contractId, ContractStatusEnum.ShipFail.ToString());
                    }

                    scope.Complete();

                    await _notificationService.SendNotificationToManagerWhenDeliveryTaskStatusUpdated((int)deliveryDetail.DeliveryTask.ManagerId,
                                                                                              deliveryDetail.DeliveryTask.ContractAddress,
                                                                                              EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Fail),
                                                                                              deliveryDetail?.DeliveryTask?.DeliveryTaskId.ToString() ?? null);
                    await _notificationService.SendNotificationToCustomerWhenDeliveryTaskStatusUpdated(
                     (int)deliveryDetail.DeliveryTask.CustomerId,
                     deliveryDetail.DeliveryTask.ContractAddress,
                     EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Fail),
                     deliveryDetail?.DeliveryTask?.DeliveryTaskId.ToString() ?? null
                        );

                    await _deliveryTaskHub.Clients.All.SendAsync("OnUpdateDeliveryTaskStatus", deliveryDetail.DeliveryTask.DeliveryTaskId);
                }
                catch (Exception ex) { }
            }
        }

        public async Task UpdateDeliveryStatusToDelivering(int deliveryTaskId, int accountId)
        {
            var deliveryDetail = await _deliveryTaskRepository.GetDeliveryTaskDetail(deliveryTaskId);

            if (deliveryDetail == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            if (accountId != deliveryDetail.DeliveryTask.StaffId)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.YouCannotChangeThisDelivery);
            }

            if (deliveryDetail.DeliveryTask.Status != DeliveryTaskStatusEnum.Created.ToString())
            {
                throw new ServiceException(MessageConstant.DeliveryTask.StatusCannotSet);
            }

            await _deliveryTaskRepository.UpdateDeliveryTaskStatus(deliveryTaskId, DeliveryTaskStatusEnum.Delivering.ToString(), accountId);

            await _notificationService.SendNotificationToManagerWhenDeliveryTaskStatusUpdated((int)deliveryDetail.DeliveryTask.ManagerId,
                                                                                                deliveryDetail.DeliveryTask.ContractAddress,
                                                                                                EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Delivering),
                                                                                                deliveryDetail?.DeliveryTask?.DeliveryTaskId.ToString() ?? null);
            await _notificationService.SendNotificationToCustomerWhenDeliveryTaskStatusUpdated(
                      (int)deliveryDetail.DeliveryTask.CustomerId,
                      deliveryDetail.DeliveryTask.ContractAddress,
                      EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Delivering),
                      deliveryDetail?.DeliveryTask?.DeliveryTaskId.ToString() ?? null);
        }

        public async Task UpdateDeliveryStatusToProcessedAfterFailure(int deliveryTaskId, int accountId)
        {
            var deliveryDetail = await _deliveryTaskRepository.GetDeliveryTaskDetail(deliveryTaskId);

            if (deliveryDetail == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            //if (accountId != deliveryDetail.DeliveryTask.ManagerId)
            //{
            //    throw new ServiceException(MessageConstant.DeliveryTask.YouCannotChangeThisDelivery);
            //}

            if (deliveryDetail.DeliveryTask.Status != DeliveryTaskStatusEnum.Fail.ToString())
            {
                throw new ServiceException(MessageConstant.DeliveryTask.StatusCannotSet);
            }
            await _deliveryTaskRepository.UpdateDeliveryTaskStatus(deliveryTaskId, DeliveryTaskStatusEnum.ProcessedAfterFailure.ToString(), accountId);
        }

        public async Task UpdateContractDeliveryStatusToProcessedAfterFailure(int contractDeliveryId, int accountId)
        {
            var contractDelivery = await _contractRepository.GetContractDelivery(contractDeliveryId);

            if (contractDelivery == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.ContractDeliveryNotFound);
            }


            if (contractDelivery.Status != ContractDeliveryStatusEnum.Fail.ToString())
            {
                throw new ServiceException(MessageConstant.DeliveryTask.StatusCannotSet);
            }

            await _contractRepository.UpdateContractDeliveryStatus(contractDeliveryId, ContractDeliveryStatusEnum.ProcessedAfterFailure.ToString());
        }

        //public async Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId)
        //{
        //    DeliveryTaskDto DeliveryTaskDto = await _deliveryTaskRepository.GetDeliveryTask(DeliveryTaskId);

        //    var account = await _accountRepository.GetAccounById(accountId);


        //    if (DeliveryTaskDto == null)
        //    {
        //        throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
        //    }

        //    if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(DeliveryTaskStatusEnum), status, true, out _))
        //    {
        //        throw new ServiceException(MessageConstant.DeliveryTask.StatusNotAvailable);
        //    }

        //    //business logic here, fix later

        //    await _deliveryTaskRepository.UpdateDeliveryTaskStatus(DeliveryTaskId, status, accountId);


        //    //if (account.RoleId == (int)AccountRoleEnum.Staff)
        //    //{
        //    //    await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated(accountId, DeliveryTaskDto.TaskTitle, status);
        //    //}

        //    if (account.RoleId == (int)AccountRoleEnum.Manager)
        //    {
        //        await _notificationService.SendNotificationToStaffWhenDeliveryTaskStatusUpdated((int)DeliveryTaskDto.StaffId, DeliveryTaskDto.ContractAddress, status);
        //    }

        //    await _deliveryTaskHub.Clients.All.SendAsync("OnUpdateDeliveryTaskStatus", DeliveryTaskId);
        //}


    }
}
