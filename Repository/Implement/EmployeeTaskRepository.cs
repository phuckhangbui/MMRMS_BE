using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.EmployeeTask;
using Repository.Interface;

namespace Repository.Implement
{
    public class EmployeeTaskRepository : IEmployeeTaskRepository
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMaintenanceRequestRepository _maintenanceRequestRepository;
        private readonly IMapper _mapper;

        public EmployeeTaskRepository(IMapper mapper, IAccountRepository accountRepository, IMaintenanceRequestRepository maintenanceRequestRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _maintenanceRequestRepository = maintenanceRequestRepository;
        }

        public async Task CreateEmployeeTaskWithRequest(int managerId, CreateEmployeeTaskCheckMachineDto createEmployeeTaskDto)
        {
            var staffAccount = await _accountRepository.GetAccounById(createEmployeeTaskDto.StaffId);

            var request = await MaintenanceRequestDao.Instance.GetMaintenanceRequest(createEmployeeTaskDto.RequestId);

            var now = DateTime.Now;
            var requestResponse = new RequestResponse
            {
                RequestId = createEmployeeTaskDto.RequestId,
                DateCreate = now,
                DateResponse = createEmployeeTaskDto.DateStart,
                Action = $"Yêu cầu của bạn đã được giao cho một nhân viên kiểm tra"
            };

            requestResponse = await RequestResponseDao.Instance.CreateAsync(requestResponse);

            var task = new EmployeeTask
            {
                TaskTitle = createEmployeeTaskDto.TaskTitle,
                Content = createEmployeeTaskDto.TaskContent,
                StaffId = createEmployeeTaskDto.StaffId,
                ManagerId = managerId,
                Type = EmployeeTaskTypeEnum.CheckMachinery.ToString(),
                DateCreate = now,
                DateStart = createEmployeeTaskDto.DateStart,
                Status = EmployeeTaskStatusEnum.Assigned.ToString(),
                Note = createEmployeeTaskDto.Note,
                RequestResponseId = requestResponse.RequestResponseId,
                ContractId = request.ContractId
            };

            var taskLog = new TaskLog
            {
                EmployeeTaskId = task.EmployeeTaskId,
                Action = $"Create and assign task to staff name {staffAccount.Name}",
                DateCreate = now,
                AccountTriggerId = managerId,
            };

            task = await EmployeeTaskDao.Instance.CreateAsync(task);

            await EmployeeTaskLogDao.Instance.CreateAsync(taskLog);

            requestResponse.EmployeeTaskId = task.EmployeeTaskId;
            await RequestResponseDao.Instance.UpdateAsync(requestResponse);

            request.Status = MaintenanceRequestStatusEnum.Assigned.ToString();
            await MaintenanceRequestDao.Instance.UpdateAsync(request);
        }

        public async Task Delete(int taskId)
        {
            var employeeTask = await EmployeeTaskDao.Instance.GetEmployeeTask(taskId);

            await EmployeeTaskDao.Instance.RemoveAsync(employeeTask);
        }

        public async Task<EmployeeTaskDto> GetEmployeeTask(int taskId)
        {
            var employeeTask = await EmployeeTaskDao.Instance.GetEmployeeTask(taskId);

            return _mapper.Map<EmployeeTaskDto>(employeeTask);
        }

        public async Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTaskByStaff(int staffId)
        {
            var list = await EmployeeTaskDao.Instance.GetEmployeeTasks();

            var resultList = list.Where(t => t.StaffId == staffId).ToList();

            return _mapper.Map<IEnumerable<EmployeeTaskDto>>(resultList);
        }

        public async Task<EmployeeTaskDisplayDetail> GetEmployeeTaskDetail(int taskId)
        {
            var employeeTask = await EmployeeTaskDao.Instance.GetEmployeeTaskDetail(taskId);

            var taskDetail = _mapper.Map<EmployeeTaskDisplayDetail>(employeeTask);

            return taskDetail;
        }

        public async Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks()
        {
            var list = await EmployeeTaskDao.Instance.GetEmployeeTasks();

            return _mapper.Map<IEnumerable<EmployeeTaskDto>>(list);
        }

        public async Task<IEnumerable<EmployeeTaskDto>> GetTaskOfStaffInADay(int staffId, DateTime date)
        {
            var list = await EmployeeTaskDao.Instance.GetEmployeeTasks();

            var staffList = list.Where(t => t.StaffId == staffId).ToList();

            var filteredList = staffList.Where(d => d.DateStart.HasValue && d.DateStart.Value.Date == date.Date).ToList();

            return _mapper.Map<IEnumerable<EmployeeTaskDto>>(filteredList);
        }

        public async Task UpdateTaskStatus(int employeeTaskId, string status, int accountId)
        {
            var employeeTask = await EmployeeTaskDao.Instance.GetEmployeeTask(employeeTaskId);

            string oldStatus = employeeTask.Status;

            employeeTask.Status = status;

            await EmployeeTaskDao.Instance.UpdateAsync(employeeTask);

            string action = $"Change status from {oldStatus} to {status}";

            var taskLog = new TaskLog
            {
                EmployeeTaskId = employeeTaskId,
                AccountTriggerId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await EmployeeTaskLogDao.Instance.CreateAsync(taskLog);
        }
    }
}
