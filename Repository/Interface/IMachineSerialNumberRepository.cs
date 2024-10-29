using DTOs.Machine;
using DTOs.MachineSerialNumber;
using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IMachineSerialNumberRepository
    {
        Task<bool> CheckMachineSerialNumbersValidToRent(List<MachineSerialNumberRentRequestDto> machineSerialNumberRentRequestDtos);
        Task CreateMachineSerialNumber(MachineSerialNumberCreateRequestDto createSerialMachineNumberDto, IEnumerable<MachineComponentDto> componentMachineList, double price, int accountId);
        Task Delete(string serialNumber);
        Task<IEnumerable<MachineSerialNumberOptionDto>> GetSerialMachineNumbersAvailableForRenting(string rentingRequestId);
        Task<bool> IsSerialNumberExist(string serialNumber);
        Task<bool> IsMachineSerialNumberHasContract(string serialNumber);
        Task Update(string serialNumber, MachineSerialNumberUpdateDto machineSerialNumberUpdateDto);
        Task<bool> CheckMachineSerialNumberValidToRequest(NewRentingRequestDto newRentingRequestDto);
        Task UpdateStatus(string serialNumber, string status, int staffId);
        Task<MachineSerialNumberDto> GetMachineSerialNumber(string serialNumber);
        Task<IEnumerable<MachineSerialNumberLogDto>> GetMachineSerialNumberLog(string serialNumber);
        Task<IEnumerable<MachineSerialNumberComponentDto>> GetMachineComponent(string serialNumber);
    }
}
