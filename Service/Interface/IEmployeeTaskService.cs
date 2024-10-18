﻿using DTOs.EmployeeTask;

namespace Service.Interface
{
    public interface IEmployeeTaskService
    {
        Task CreateEmployeeTask(int managerId, CreateEmployeeTaskDto createEmployeeTaskDto);
        Task DeleteEmployeeTask(int taskId);
        Task<EmployeeTaskDisplayDetail> GetEmployeeTaskDetail(int taskId);
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks();
        Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks(int staffId);
        Task UpdateEmployeeTaskStatus(int employeeTaskId, string status, int accountId);
    }
}
