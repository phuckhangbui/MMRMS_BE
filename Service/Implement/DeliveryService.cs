using AutoMapper;
using Common;
using Common.Enum;
using DTOs.Contract;
using DTOs.Delivery;
using DTOs.DeliveryTask;
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
        private readonly IHubContext<DeliveryTaskHub> _DeliveryTaskHub;
        private readonly IMapper _mapper;

        public DeliveryService(IDeliveryTaskRepository DeliveryTaskRepository, IMachineTaskRepository MachineTaskRepository, IAccountRepository accountRepository, IHubContext<DeliveryTaskHub> DeliveryTaskHub, INotificationService notificationService, IContractRepository contractRepository, IRentingRequestRepository rentingRequestRepository, IMapper mapper, IMachineSerialNumberRepository machineSerialNumberRepository)
        {
            _deliveryTaskRepository = DeliveryTaskRepository;
            _machineTaskRepository = MachineTaskRepository;
            _accountRepository = accountRepository;
            _DeliveryTaskHub = DeliveryTaskHub;
            _notificationService = notificationService;
            _contractRepository = contractRepository;
            _rentingRequestRepository = rentingRequestRepository;
            _mapper = mapper;
            _machineSerialNumberRepository = machineSerialNumberRepository;
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


                if (contract.Status != ContractStatusEnum.Signed.ToString())
                {
                    throw new ServiceException(MessageConstant.Contract.ContractNotValidToDelivery);
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

            await _notificationService.SendNotificationToStaffWhenAssignDeliveryTask((int)deliveryDto.StaffId, contractAddress, (DateTime)deliveryDto.DateShip);

            await _DeliveryTaskHub.Clients.All.SendAsync("OnCreateDeliveryTaskToStaff", deliveryDto.DeliveryTaskId);
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


            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _deliveryTaskRepository.CompleteFullyAllDeliveryTask(staffUpdateDeliveryTaskDto);

                    string contractId = "";
                    foreach (var contractDelivery in deliveryDetail.ContractDeliveries)
                    {
                        contractId = contractDelivery.ContractId;

                        await _contractRepository.UpdateContractStatus(contractId, ContractStatusEnum.Renting.ToString());

                        await _machineSerialNumberRepository.UpdateStatus(contractDelivery.SerialNumber, MachineSerialNumberStatusEnum.Renting.ToString(), accountId);
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

                    await _notificationService.SendNotificationToManagerWhenDeliveryTaskStatusUpdated((int)deliveryDetail.DeliveryTask.ManagerId, deliveryDetail.DeliveryTask.ContractAddress, EnumExtensions.ToVietnamese(DeliveryTaskStatusEnum.Completed));

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(ex.ToString());
                }
            }
        }

        public async Task UpdateDeliveryStatusToDelivering(int deliveryTaskId, int accountId)
        {
            var delivery = await _deliveryTaskRepository.GetDeliveryTask(deliveryTaskId);

            if (delivery == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            if (accountId != delivery.StaffId)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.YouCannotChangeThisDelivery);
            }

            if (delivery.Status != DeliveryTaskStatusEnum.Created.ToString())
            {
                throw new ServiceException(MessageConstant.DeliveryTask.StatusCannotSet);
            }

            await _deliveryTaskRepository.UpdateDeliveryTaskStatus(deliveryTaskId, DeliveryTaskStatusEnum.Delivering.ToString(), accountId);

        }

        public async Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId)
        {
            DeliveryTaskDto DeliveryTaskDto = await _deliveryTaskRepository.GetDeliveryTask(DeliveryTaskId);

            var account = await _accountRepository.GetAccounById(accountId);


            if (DeliveryTaskDto == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(DeliveryTaskStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.DeliveryTask.StatusNotAvailable);
            }

            //business logic here, fix later

            await _deliveryTaskRepository.UpdateDeliveryTaskStatus(DeliveryTaskId, status, accountId);


            //if (account.RoleId == (int)AccountRoleEnum.Staff)
            //{
            //    await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated(accountId, DeliveryTaskDto.TaskTitle, status);
            //}

            if (account.RoleId == (int)AccountRoleEnum.Manager)
            {
                await _notificationService.SendNotificationToStaffWhenDeliveryTaskStatusUpdated((int)DeliveryTaskDto.StaffId, DeliveryTaskDto.ContractAddress, status);
            }

            await _DeliveryTaskHub.Clients.All.SendAsync("OnUpdateDeliveryTaskStatus", DeliveryTaskId);
        }


    }
}
