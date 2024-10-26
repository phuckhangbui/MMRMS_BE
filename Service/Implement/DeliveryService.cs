using Common;
using Common.Enum;
using DTOs.Delivery;
using DTOs.DeliveryTask;
using DTOs.MachineTask;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;

namespace Service.Implement
{
    public class DeliveryService : IDeliverService
    {

        private readonly IDeliveryTaskRepository _deliveryTaskRepository;
        private readonly IMachineTaskRepository _machineTaskRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IContractRepository _contractRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<DeliveryTaskHub> _DeliveryTaskHub;

        public DeliveryService(IDeliveryTaskRepository DeliveryTaskRepository, IMachineTaskRepository MachineTaskRepository, IAccountRepository accountRepository, IHubContext<DeliveryTaskHub> DeliveryTaskHub, INotificationService notificationService, IContractRepository contractRepository)
        {
            _deliveryTaskRepository = DeliveryTaskRepository;
            _machineTaskRepository = MachineTaskRepository;
            _accountRepository = accountRepository;
            _DeliveryTaskHub = DeliveryTaskHub;
            _notificationService = notificationService;
            _contractRepository = contractRepository;
        }

        public async Task CreateDeliveryTask(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto)
        {
            var accountDto = await _accountRepository.GetAccounById(createDeliveryTaskDto.StaffId);

            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            var taskList = await _machineTaskRepository.GetTaskOfStaffInADay(createDeliveryTaskDto.StaffId, createDeliveryTaskDto.DateShip)
                                        ?? Enumerable.Empty<MachineTaskDto>();

            var deliveryTaskList = await _deliveryTaskRepository.GetDeliveriesOfStaffInADay(createDeliveryTaskDto.StaffId, createDeliveryTaskDto.DateShip)
                                        ?? Enumerable.Empty<DeliveryTaskDto>();

            int taskCounter = taskList.Count() + deliveryTaskList.Count();


            if (taskCounter >= GlobalConstant.MaxTaskLimitADayContract)
            {
                throw new ServiceException(MessageConstant.MachineTask.ReachMaxTaskLimit);
            }

            string rentingRequestId = null;

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
            }

            var deliveryDto = await _deliveryTaskRepository.CreateDeliveryTaskToStaff(managerId, createDeliveryTaskDto);

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
            return await _deliveryTaskRepository.GetDeliveryTaskDetail(deliveryTaskId);
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
