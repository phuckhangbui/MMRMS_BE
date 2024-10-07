using Common;
using Common.Enum;
using DTOs.Delivery;
using DTOs.EmployeeTask;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;

namespace Service.Implement
{
    public class DeliveryService : IDeliveryService
    {

        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<DeliveryHub> _deliveryHub;

        public DeliveryService(IDeliveryRepository deliveryRepository, IEmployeeTaskRepository employeeTaskRepository, IAccountRepository accountRepository, IHubContext<DeliveryHub> deliveryHub, INotificationService notificationService)
        {
            _deliveryRepository = deliveryRepository;
            _employeeTaskRepository = employeeTaskRepository;
            _accountRepository = accountRepository;
            _deliveryHub = deliveryHub;
            _notificationService = notificationService;
        }

        public async Task AssignDelivery(int managerId, AssignDeliveryDto assignDeliveryDto)
        {
            var deliveryDto = await _deliveryRepository.GetDelivery(assignDeliveryDto.DeliveryId);

            if (deliveryDto == null)
            {
                throw new ServiceException(MessageConstant.Delivery.DeliveryNotFound);
            }

            var accountDto = await _accountRepository.GetAccounById(assignDeliveryDto.StaffId);

            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            var taskList = await _employeeTaskRepository.GetTaskOfStaffInADay(assignDeliveryDto.StaffId, assignDeliveryDto.DateShip)
                                        ?? Enumerable.Empty<EmployeeTaskDto>();

            var deliveryList = await _deliveryRepository.GetDeliveriesOfStaffInADay(assignDeliveryDto.StaffId, assignDeliveryDto.DateShip)
                                        ?? Enumerable.Empty<DeliveryDto>();

            int taskCounter = taskList.Count() + deliveryList.Count();


            if (taskCounter >= GlobalConstant.MaxTaskLimitADayContract)
            {
                throw new ServiceException(MessageConstant.EmployeeTask.ReachMaxTaskLimit);
            }

            await _deliveryRepository.AssignDeliveryToStaff(managerId, assignDeliveryDto);

            await _notificationService.SendNotificationToStaffWhenAssignDelivery(assignDeliveryDto.StaffId, deliveryDto.ContractAddress, assignDeliveryDto.DateShip);

            await _deliveryHub.Clients.All.SendAsync("OnAssignDeliveryToStaff", deliveryDto.DeliveryId);
        }

        public async Task<IEnumerable<DeliveryDto>> GetDeliveries()
        {
            return await _deliveryRepository.GetDeliveries();
        }

        public async Task<IEnumerable<DeliveryDto>> GetDeliveries(int staffId)
        {
            return await _deliveryRepository.GetDeliveriesForStaff(staffId);
        }

        public async Task UpdateDeliveryStatus(int deliveryId, string status, int accountId)
        {
            DeliveryDto deliveryDto = await _deliveryRepository.GetDelivery(deliveryId);

            var account = await _accountRepository.GetAccounById(accountId);


            if (deliveryDto == null)
            {
                throw new ServiceException(MessageConstant.Delivery.DeliveryNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(DeliveryStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.Delivery.StatusNotAvailable);
            }

            //business logic here, fix later

            await _deliveryRepository.UpdateDeliveryStatus(deliveryId, status, accountId);


            //if (account.RoleId == (int)AccountRoleEnum.Staff)
            //{
            //    await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated(accountId, deliveryDto.TaskTitle, status);
            //}

            if (account.RoleId == (int)AccountRoleEnum.Manager)
            {
                await _notificationService.SendNotificationToStaffWhenDeliveryStatusUpdated((int)deliveryDto.StaffId, deliveryDto.ContractAddress, status);
            }

            await _deliveryHub.Clients.All.SendAsync("OnUpdateDeliveryStatus", deliveryId);
        }
    }
}
