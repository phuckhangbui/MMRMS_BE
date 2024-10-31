
using DTOs.MachineTask;

namespace Repository.Interface
{
    public interface IMachineTaskRepository
    {
        Task<MachineTaskDto> CreateMachineTaskWithRequest(int managerId, CreateMachineTaskCheckMachineDto createMachineTaskDto);
        Task Delete(int taskId);
        Task<MachineTaskDto> GetMachineTask(int taskId);
        Task<IEnumerable<MachineTaskDto>> GetMachineTaskByStaff(int staffId);
        Task<MachineTaskDisplayDetail> GetMachineTaskDetail(int taskId);
        Task<IEnumerable<MachineTaskDto>> GetMachineTasks();
        Task<IEnumerable<MachineTaskDto>> GetMachineTasksForStaff(int staffId, DateOnly dateStart, DateOnly dateEnd);
        Task<IEnumerable<MachineTaskDto>> GetMachineTasksInADate(DateOnly date);
        Task<IEnumerable<MachineTaskDto>> GetTaskOfStaffInADay(int staffId, DateTime date);
        Task UpdateTaskStatus(int machineTaskId, string status, int accountId, string? confirmationPictureUrl);
    }
}
