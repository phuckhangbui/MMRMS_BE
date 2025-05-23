﻿using Common;
using Common.Enum;
using DTOs.Contract;
using DTOs.Delivery;
using DTOs.MachineTask;
using DTOs.RentingRequest;
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
        private readonly IMachineRepository _machineRepository;
        private readonly ISettingsService _settingsService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IHubContext<DeliveryTaskHub> _deliveryTaskHub;
        private readonly IBackground _background;

        public DeliveryService(IDeliveryTaskRepository DeliveryTaskRepository, IMachineTaskRepository MachineTaskRepository, IAccountRepository accountRepository, IHubContext<DeliveryTaskHub> DeliveryTaskHub, INotificationService notificationService, IContractRepository contractRepository, IRentingRequestRepository rentingRequestRepository, IMachineSerialNumberRepository machineSerialNumberRepository, IBackground background, IMachineRepository machineRepository, ISettingsService settingsService, ICloudinaryService cloudinaryService)
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
            _machineRepository = machineRepository;
            _settingsService = settingsService;
            _cloudinaryService = cloudinaryService;
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

            var settings = await _settingsService.GetSettingsAsync();

            int maxTaskLimit = settings
                     .FirstOrDefault(s => s.Name == "maxTaskADay")?.Value is int limit
                         ? limit
                         : int.TryParse(settings.FirstOrDefault(s => s.Name == "maxTaskADay")?.Value.ToString(), out int parsedValue)
                             ? parsedValue
                             : GlobalConstant.MaxTaskLimitADay;

            if (taskCounter >= maxTaskLimit)
            {
                throw new ServiceException(MessageConstant.MachineTask.ReachMaxTaskLimit);
            }

            if (createDeliveryTaskDto.ContractIdList.Count() < createDeliveryTaskDto.DeliveryVehicleCounter)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.VehicleIsBiggerThanNumberOfMachine);
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

                var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(contract.SerialNumber);

                if (machineSerialNumber == null)
                {
                    throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
                }

                if (machineSerialNumber.Status != MachineSerialNumberStatusEnum.Available.ToString()
                    && machineSerialNumber.Status != MachineSerialNumberStatusEnum.Reserved.ToString())
                {
                    throw new ServiceException(MessageConstant.DeliveryTask.MachineSerialNumberNotInAvailableStatus
                                                    + EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(machineSerialNumber.Status));
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

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveriesForCustomer(int customerId)
        {
            return await _deliveryTaskRepository.GetDeliveryTasksForCustomer(customerId);
        }

        public async Task<DeliveryTaskDetailDto> GetDeliveryDetail(int deliveryTaskId)
        {
            try
            {

                var deliveryDetail = await _deliveryTaskRepository.GetDeliveryTaskDetail(deliveryTaskId);

                var updatedContractDeliveryList = new List<ContractDeliveryDto>();

                foreach (var contractDelivery in deliveryDetail.ContractDeliveries)
                {
                    var machine = await _machineRepository.GetMachineByMachineSerial(contractDelivery.SerialNumber);

                    if (machine != null)
                    {
                        contractDelivery.MachineId = machine.MachineId;
                        contractDelivery.MachineName = machine.MachineName;
                        contractDelivery.MachineModel = machine.Model;
                        contractDelivery.Weight = machine.Weight;
                    }

                    updatedContractDeliveryList.Add(contractDelivery);
                }

                if (!updatedContractDeliveryList.IsNullOrEmpty())
                {
                    deliveryDetail.ContractDeliveries = updatedContractDeliveryList;
                }

                return deliveryDetail;
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
                    var base64Images = staffUpdateDeliveryTaskDto.ContractDeliveries
                        .Where(contractDelivery => !string.IsNullOrEmpty(contractDelivery.PictureUrl))
                        .Select(contractDelivery => contractDelivery.PictureUrl)
                        .ToArray();

                    if (base64Images.Length == 0)
                    {
                        throw new ServiceException(MessageConstant.DeliveryTask.ImageIsRequired);
                    }

                    var uploadedImageUrls = await _cloudinaryService.UploadImageToCloudinary(base64Images);

                    var contractDeliveriesList = staffUpdateDeliveryTaskDto.ContractDeliveries.ToList();

                    for (int i = 0; i < uploadedImageUrls.Length; i++)
                    {
                        contractDeliveriesList[i].PictureUrl = uploadedImageUrls[i];
                    }

                    staffUpdateDeliveryTaskDto.ContractDeliveries = contractDeliveriesList;

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

            string contractId = deliveryDetail.ContractDeliveries.FirstOrDefault().ContractId;

            var rentingRequest = await _rentingRequestRepository.GetRentingRequestByContractId(contractId);

            if (rentingRequest == null)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestNotFound);
            }

            double refundDeliveryAmountPerContract = this.GetRefundAmountPerContract(rentingRequest, staffUpdateDeliveryTaskDto, deliveryDetail);

            foreach (var contractDeliveryDto in staffUpdateDeliveryTaskDto.ContractDeliveries)
            {
                var contractDelivery = deliveryDetail.ContractDeliveries
                    .FirstOrDefault(cd => cd.ContractDeliveryId == contractDeliveryDto.ContractDeliveryId);

                if (contractDelivery != null)
                {
                    contractId = contractDelivery.ContractId;

                    if (contractDeliveryDto.IsSuccess)
                    {


                        await _contractRepository.UpdateContractStatusToRenting(contractId, refundDeliveryAmountPerContract);
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

            string contractId = deliveryDetail.ContractDeliveries.FirstOrDefault().ContractId;

            var rentingRequest = await _rentingRequestRepository.GetRentingRequestByContractId(contractId);

            if (rentingRequest == null)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestNotFound);
            }


            double refundDeliveryAmountPerContract = this.GetRefundAmountPerContract(rentingRequest, staffUpdateDeliveryTaskDto, deliveryDetail);

            contractId = "";

            foreach (var contractDelivery in deliveryDetail.ContractDeliveries)
            {
                contractId = contractDelivery.ContractId;

                await _contractRepository.UpdateContractStatusToRenting(contractId, refundDeliveryAmountPerContract);

                await _machineSerialNumberRepository.UpdateStatus(contractDelivery.SerialNumber, MachineSerialNumberStatusEnum.Renting.ToString(), accountId);

                //Schedule Background Job
                var contractDto = await _contractRepository.GetContractById(contractId);
                TimeSpan timeUntilEnd = (TimeSpan)(contractDto.DateEnd - DateTime.Now);
                _background.CompleteContractOnTimeJob(contractId, timeUntilEnd);
            }

            var contract = await _contractRepository.GetContractById(contractId);
            if (contract != null)
            {
                var rentingRequestDetail = await _rentingRequestRepository.GetRentingRequestDetailById(contract.RentingRequestId);

                if (rentingRequestDetail.Contracts != null && rentingRequestDetail.Contracts.All(c => c.Status == ContractStatusEnum.Renting.ToString()))
                {
                    await _rentingRequestRepository.UpdateRentingRequestStatus(rentingRequestDetail.RentingRequestId, RentingRequestStatusEnum.Shipped.ToString());

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

        private double GetRefundAmountPerContract(RentingRequestDto rentingRequest, StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto, DeliveryTaskDetailDto deliveryDetail)
        {
            double actualDeliveryAmountPerContract = 0;
            double refundDeliveryAmountPerContract = 0;

            double? finalDeliveryAmountInThisTrip = rentingRequest?.ShippingPricePerKm * rentingRequest?.ShippingDistance * deliveryDetail?.DeliveryTask.DeliveryVehicleCounter;

            double? originalDeliveryAmountPerContract = rentingRequest?.ShippingPricePerKm * rentingRequest?.ShippingDistance;


            if (finalDeliveryAmountInThisTrip != null && originalDeliveryAmountPerContract != null)
            {
                if (!staffUpdateDeliveryTaskDto.ContractDeliveries.IsNullOrEmpty()
                    && staffUpdateDeliveryTaskDto?.ContractDeliveries?.Where(c => c.IsSuccess).Count() > 0)
                {
                    actualDeliveryAmountPerContract = (double)finalDeliveryAmountInThisTrip
                                                / staffUpdateDeliveryTaskDto.ContractDeliveries.Where(c => c.IsSuccess).Count();

                    if (actualDeliveryAmountPerContract < 0)
                    {
                        actualDeliveryAmountPerContract = 0;
                    }

                    refundDeliveryAmountPerContract = (double)originalDeliveryAmountPerContract - actualDeliveryAmountPerContract;

                    if (refundDeliveryAmountPerContract < 0)
                    {
                        refundDeliveryAmountPerContract = 0;
                    }
                }
            }

            return refundDeliveryAmountPerContract;
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
                    scope.Complete();

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

            await _deliveryTaskHub.Clients.All.SendAsync("OnUpdateDeliveryTaskStatus", deliveryDetail.DeliveryTask.DeliveryTaskId);
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
    }
}
