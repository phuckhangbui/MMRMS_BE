using DTOs.MachineComponentStatus;

namespace Repository.Interface
{
    public interface IMachineSerialNumberComponentRepository
    {
        Task<MachineSerialNumberComponentDto> GetComponent(int machineSerialNumberComponentId);
        Task UpdateComponentStatus(int machineSerialNumberComponentId, string status, int accountId);
    }
}
