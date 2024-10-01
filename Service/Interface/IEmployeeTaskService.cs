using DTOs.EmployeeTask;

namespace Service.Interface
{
    public interface IEmployeeTaskService
    {
        Task DeleteEmployeeTask(int taskId);
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks();
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks(int staffId);
        Task UpdateEmployeeTaskStatus(int employeeTaskId, string status, int accountId);
    }
}
