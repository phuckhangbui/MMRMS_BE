
using DTOs.EmployeeTask;

namespace Repository.Interface
{
    public interface IEmployeeTaskRepository
    {
        Task CreateEmployeeTaskWithRequest(int managerId, CreateEmployeeTaskDto createEmployeeTaskDto);
        Task Delete(int taskId);
        Task<EmployeeTaskDto> GetEmployeeTask(int taskId);
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTaskByStaff(int staffId);
        Task<EmployeeTaskDisplayDetail> GetEmployeeTaskDetail(int taskId);
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks();
        Task<IEnumerable<EmployeeTaskDto>> GetTaskOfStaffInADay(int staffId, DateTime date);
        Task UpdateTaskStatus(int employeeTaskId, string status, int accountId);
    }
}
