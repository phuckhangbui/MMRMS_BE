using DTOs.MachineTask;

namespace Service.Interface
{
    public interface IMachineTaskService
    {
        Task CreateMachineTaskCheckMachine(int managerId, CreateMachineTaskCheckMachineDto createMachineTaskDto);
        Task CreateMachineTaskProcessComponentReplacementTicket(int managerId, CreateMachineTaskProcessComponentReplacementTickett createMachineTaskDto);
        Task DeleteMachineTask(int taskId);
        Task<MachineTaskDisplayDetail> GetMachineTaskDetail(int taskId);
        Task<IEnumerable<MachineTaskDto>> GetMachineTasks();
        Task<IEnumerable<MachineTaskDto>> GetMachineTasks(int staffId);
        Task StaffCheckMachineSuccess(int taskId, int staffId);
        Task StaffReplaceComponentSuccess(int taskId, int staffId);
        Task UpdateMachineTaskStatus(int MachineTaskId, string status, int accountId);
    }
}
