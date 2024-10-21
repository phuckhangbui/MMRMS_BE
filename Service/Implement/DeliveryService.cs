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
    public class DeliveryService : IDeliverService
    {

        private readonly IDeliveryTaskRepository _DeliveryTaskRepository;
        private readonly IMachineTaskRepository _MachineTaskRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<DeliveryTaskHub> _DeliveryTaskHub;

        public DeliveryService(IDeliveryTaskRepository DeliveryTaskRepository, IMachineTaskRepository MachineTaskRepository, IAccountRepository accountRepository, IHubContext<DeliveryTaskHub> DeliveryTaskHub, INotificationService notificationService)
        {
            _DeliveryTaskRepository = DeliveryTaskRepository;
            _MachineTaskRepository = MachineTaskRepository;
            _accountRepository = accountRepository;
            _DeliveryTaskHub = DeliveryTaskHub;
            _notificationService = notificationService;
        }

        public async Task AssignDeliveryTask(int managerId, AssignDeliveryTaskDto assignDeliveryTaskDto)
        {
            var DeliveryTaskDto = await _DeliveryTaskRepository.GetDeliveryTask(assignDeliveryTaskDto.DeliveryTaskId);

            if (DeliveryTaskDto == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            var accountDto = await _accountRepository.GetAccounById(assignDeliveryTaskDto.StaffId);

            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            var taskList = await _MachineTaskRepository.GetTaskOfStaffInADay(assignDeliveryTaskDto.StaffId, assignDeliveryTaskDto.DateShip)
                                        ?? Enumerable.Empty<MachineTaskDto>();

            var DeliveryTaskList = await _DeliveryTaskRepository.GetDeliveriesOfStaffInADay(assignDeliveryTaskDto.StaffId, assignDeliveryTaskDto.DateShip)
                                        ?? Enumerable.Empty<DeliveryTaskDto>();

            int taskCounter = taskList.Count() + DeliveryTaskList.Count();


            if (taskCounter >= GlobalConstant.MaxTaskLimitADayContract)
            {
                throw new ServiceException(MessageConstant.MachineTask.ReachMaxTaskLimit);
            }

            await _DeliveryTaskRepository.AssignDeliveryTaskToStaff(managerId, assignDeliveryTaskDto);

            await _notificationService.SendNotificationToStaffWhenAssignDeliveryTask(assignDeliveryTaskDto.StaffId, DeliveryTaskDto.ContractAddress, assignDeliveryTaskDto.DateShip);

            await _DeliveryTaskHub.Clients.All.SendAsync("OnAssignDeliveryTaskToStaff", DeliveryTaskDto.DeliveryTaskId);
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveries()
        {
            return await _DeliveryTaskRepository.GetDeliveries();
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveries(int staffId)
        {
            return await _DeliveryTaskRepository.GetDeliveriesForStaff(staffId);
        }

        public async Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId)
        {
            DeliveryTaskDto DeliveryTaskDto = await _DeliveryTaskRepository.GetDeliveryTask(DeliveryTaskId);

            var account = await _accountRepository.GetAccounById(accountId);


            if (DeliveryTaskDto == null)
            {
                throw new ServiceException(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(DeliveryTasktatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.DeliveryTask.StatusNotAvailable);
            }

            //business logic here, fix later

            await _DeliveryTaskRepository.UpdateDeliveryTaskStatus(DeliveryTaskId, status, accountId);


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
