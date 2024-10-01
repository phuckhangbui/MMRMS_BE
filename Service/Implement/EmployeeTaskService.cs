using DTOs.EmployeeTask;
using Service.Interface;

namespace Service.Implement
{
    public class EmployeeTaskService : IEmployeeTaskService
    {
        public Task DeleteEmployeeTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks(int staffId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEmployeeTaskStatus(int employeeTaskId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
