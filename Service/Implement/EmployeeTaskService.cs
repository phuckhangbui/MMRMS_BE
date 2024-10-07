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
    public class EmployeeTaskService : IEmployeeTaskService
    {
        private readonly IEmployeeTaskRepository _employeeTaskRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMaintenanceRequestRepository _maintenanceRequestRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<EmployeeTaskHub> _employeeTaskHub;

        public EmployeeTaskService(IEmployeeTaskRepository employeeTaskRepository, IHubContext<EmployeeTaskHub> employeeTaskHub, INotificationService notificationService, IAccountRepository accountRepository, IMaintenanceRequestRepository maintenanceRequestRepository, IDeliveryRepository deliveryRepository)
        {
            _employeeTaskRepository = employeeTaskRepository;
            _employeeTaskHub = employeeTaskHub;
            _notificationService = notificationService;
            _accountRepository = accountRepository;
            _maintenanceRequestRepository = maintenanceRequestRepository;
            _deliveryRepository = deliveryRepository;
        }

        public async Task CreateEmployeeTask(int managerId, CreateEmployeeTaskDto createEmployeeTaskDto)
        {
            var staff = await _accountRepository.GetAccounById(createEmployeeTaskDto.StaffId);

            if (staff == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            var requestDto = await _maintenanceRequestRepository.GetMaintenanceRequest(createEmployeeTaskDto.RequestId);

            if (requestDto == null)
            {
                throw new ServiceException(MessageConstant.MaintenanceRequest.RequestNotFound);
            }
            var staffTaskList = await _employeeTaskRepository.GetTaskOfStaffInADay(createEmployeeTaskDto.StaffId, createEmployeeTaskDto.DateStart)
                                        ?? Enumerable.Empty<EmployeeTaskDto>();
            var staffDeliveryList = await _deliveryRepository.GetDeliveriesOfStaffInADay(createEmployeeTaskDto.StaffId, createEmployeeTaskDto.DateStart)
                                        ?? Enumerable.Empty<DeliveryDto>();

            int taskCounter = staffDeliveryList.Count() + staffTaskList.Count();

            if (taskCounter >= GlobalConstant.MaxTaskLimitADayContract)
            {
                throw new ServiceException(MessageConstant.EmployeeTask.ReachMaxTaskLimit);
            }

            await _employeeTaskRepository.CreateEmployeeTaskWithRequest(managerId, createEmployeeTaskDto);

            await _notificationService.SendNotificationToStaffWhenAssignTaskToMaintenance(createEmployeeTaskDto.StaffId, requestDto.ContractAddress, createEmployeeTaskDto.DateStart);

            await _employeeTaskHub.Clients.All.SendAsync("OnCreateEmployeeTask");
        }


        // currently comment out the controller, because task always have staffId when created
        public async Task DeleteEmployeeTask(int taskId)
        {
            var employeeTask = await _employeeTaskRepository.GetEmployeeTask(taskId);

            if (employeeTask == null)
            {
                throw new ServiceException(MessageConstant.EmployeeTask.TaskNotFound);
            }

            if (employeeTask.StaffId != null)
            {
                throw new ServiceException(MessageConstant.EmployeeTask.CannotDeleted);
            }

            //need to check logic of the RequestResponse
            await _employeeTaskRepository.Delete(taskId);

            await _employeeTaskHub.Clients.All.SendAsync("OnDeleteEmployeeTask", taskId);
        }

        public async Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks()
        {
            var list = await _employeeTaskRepository.GetEmployeeTasks();

            return list;
        }

        public async Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks(int staffId)
        {
            var list = await _employeeTaskRepository.GetEmployeeTaskByStaff(staffId);

            return list;
        }

        public async Task UpdateEmployeeTaskStatus(int employeeTaskId, string status, int accountId)
        {
            var employeeTask = await _employeeTaskRepository.GetEmployeeTask(employeeTaskId);

            var account = await _accountRepository.GetAccounById(accountId);

            if (employeeTask == null)
            {
                throw new ServiceException(MessageConstant.EmployeeTask.TaskNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(EmployeeTaskStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.EmployeeTask.StatusNotAvailable);
            }
            //logic here


            await _employeeTaskRepository.UpdateTaskStatus(employeeTaskId, status, accountId);

            if (account.RoleId == (int)AccountRoleEnum.Staff)
            {
                await _notificationService.SendNotificationToManagerWhenTaskStatusUpdated((int)employeeTask.ManagerId, employeeTask.TaskTitle, status);
            }

            if (account.RoleId == (int)AccountRoleEnum.Manager)
            {
                await _notificationService.SendNotificationToStaffWhenTaskStatusUpdated((int)employeeTask.StaffId, employeeTask.TaskTitle, status);
            }

            await _employeeTaskHub.Clients.All.SendAsync("OnUpdateEmployeeTaskStatus", employeeTaskId);
        }
    }
}
