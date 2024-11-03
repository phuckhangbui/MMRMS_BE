using DTOs.MachineSerialNumber;

namespace Service.Interface
{
    public interface IMachineSerialNumberService
    {
        Task CreateMachineSerialNumber(MachineSerialNumberCreateRequestDto dto, int accountId);
        Task Delete(string serialNumber);
        Task Update(string serialNumber, MachineSerialNumberUpdateDto machineSerialNumberUpdateDto);
        Task<IEnumerable<MachineSerialNumberLogDto>> GetDetailLog(string serialNumber);
        Task<IEnumerable<MachineSerialNumberComponentDto>> GetSerialNumberComponents(string serialNumber);
        Task ToggleStatus(string serialNumber, int staffId);
        Task UpdateMachineSerialNumberComponentStatusToBroken(int machineSerialNumberComponentId, int accountId);
    }
}
