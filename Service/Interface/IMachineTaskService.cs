﻿using DTOs.MachineTask;

namespace Service.Interface
{
    public interface IMachineTaskService
    {
        Task CreateMachineTaskCheckMachine(int managerId, CreateMachineTaskCheckRequestDto createMachineTaskDto);
        Task CreateMachineTaskCheckMachineContractTermination(int managerId, CreateMachineTaskContractTerminationDto createMachineTaskDto);
        Task CreateMachineTaskCheckMachineDeliveryFail(int managerId, CreateMachineTaskDeliveryFailDto createMachineTaskDto);
        Task<MachineTaskDisplayDetail> GetMachineTaskDetail(int taskId);
        Task<IEnumerable<MachineTaskDto>> GetMachineTasks();
        Task<IEnumerable<MachineTaskDto>> GetMachineTasks(int staffId);
        Task StaffCheckMachineSuccess(int taskId, int staffId, string? confirmationPictureUrl);
    }
}
