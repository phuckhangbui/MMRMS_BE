using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.EmployeeTask;
using Repository.Interface;

namespace Repository.Implement
{
    public class EmployeeTaskRepository : IEmployeeTaskRepository
    {
        private readonly IMapper _mapper;
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

        public async Task<IEnumerable<EmployeeTaskDto>> GetEmployeeTasks()
        {
            var list = await EmployeeTaskDao.Instance.GetEmployeeTasks();

            return _mapper.Map<IEnumerable<EmployeeTaskDto>>(list);
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
                AccountId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await EmployeeTaskLogDao.Instance.CreateAsync(taskLog);
        }
    }
}
