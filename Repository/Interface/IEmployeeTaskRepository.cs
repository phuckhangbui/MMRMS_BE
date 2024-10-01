﻿
using DTOs.EmployeeTask;

namespace Repository.Interface
{
    public interface IEmployeeTaskRepository
    {
        Task Delete(int taskId);
        Task<EmployeeTaskDto> GetEmployeeTask(int taskId);
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTaskByStaff(int staffId);
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks();
        Task UpdateTaskStatus(int employeeTaskId, string status, int accountId);
    }
}
