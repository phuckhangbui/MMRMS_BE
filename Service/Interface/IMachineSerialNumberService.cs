using DTOs.MachineSerialNumber;

namespace Service.Interface
{
    public interface IMachineSerialNumberService
    {
        Task CreateMachineSerialNumber(MachineSerialNumberCreateRequestDto dto, int accountId);
        Task Delete(string serialNumber);
        Task Update(string serialNumber, MachineSerialNumberUpdateDto machineSerialNumberUpdateDto);
        //Task UpdateStatus(string serialNumber, string status);
        Task<IEnumerable<MachineSerialNumberOptionDto>> GetSerialMachineNumbersAvailableForRenting(string rentingRequestId);
        Task<IEnumerable<MachineSerialNumberLogDto>> GetDetailLog(string serialNumber);
        Task<IEnumerable<MachineComponentStatusDto>> GetSerialNumberComponentStatus(string serialNumber);
        Task ToggleStatus(string serialNumber, int staffId);
    }
}
