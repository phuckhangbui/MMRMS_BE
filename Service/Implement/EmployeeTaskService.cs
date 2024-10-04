using Common;
using Common.Enum;
using DTOs.EmployeeTask;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class EmployeeTaskService : IEmployeeTaskService
    {
        private readonly IEmployeeTaskRepository _employeeTaskRepository;

        public EmployeeTaskService(IEmployeeTaskRepository employeeTaskRepository)
        {
            _employeeTaskRepository = employeeTaskRepository;
        }

        public Task CreateEmployeeTask(int managerId, CreateEmployeeTaskDto createEmployeeTaskDto)
        {
            throw new NotImplementedException();
        }

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

            await _employeeTaskRepository.Delete(taskId);
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
        }
    }
}
