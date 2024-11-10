using DTOs.MachineSerialNumber;

namespace Service.Interface
{
    public interface IMachineSerialNumberService
    {
        Task CreateMachineSerialNumber(MachineSerialNumberCreateRequestDto dto, int accountId);
        Task Delete(string serialNumber);
        Task<IEnumerable<MachineSerialNumberLogDto>> GetDetailLog(string serialNumber);
        Task<IEnumerable<MachineSerialNumberComponentDto>> GetSerialNumberComponents(string serialNumber);
        Task ToggleStatus(string serialNumber, int staffId);
        Task UpdateMachineSerialNumberComponentStatusToBrokenWhileInStore(int machineSerialNumberComponentId, int accountId, string note);
        Task UpdateMachineSerialNumberComponentStatusToNormalWhileInStore(int machineSerialNumberComponentId, int staffId, bool isDeductFromComponentStorage, int quantity, string note);
        Task MoveSerialMachineToMaintenanceStatus(string serialNumber, int staffId, string note);
        Task MoveSerialMachineToActiveStatus(string serialNumber, int staffId, string note);
    }
}
