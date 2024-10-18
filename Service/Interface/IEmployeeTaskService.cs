using DTOs.EmployeeTask;

namespace Service.Interface
{
    public interface IEmployeeTaskService
    {
        Task CreateEmployeeTaskCheckMachine(int managerId, CreateEmployeeTaskCheckMachineDto createEmployeeTaskDto);
        Task CreateEmployeeTaskProcessMaintenanceTicket(int managerId, CreateEmployeeTaskProcessMaintenanceTickett createEmployeeTaskDto);
        Task DeleteEmployeeTask(int taskId);
        Task<EmployeeTaskDisplayDetail> GetEmployeeTaskDetail(int taskId);
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks();
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks(int staffId);
        Task UpdateEmployeeTaskStatus(int employeeTaskId, string status, int accountId);
    }
}
