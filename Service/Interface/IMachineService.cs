using DTOs.Machine;
using DTOs.MachineSerialNumber;

namespace Service.Interface
{
    public interface IMachineService
    {
        Task<IEnumerable<MachineViewDto>> GetMachineList();
        Task<IEnumerable<MachineDto>> GetActiveMachines();
        Task<IEnumerable<MachineDto>> GetTop8LatestMachineList();
        Task<IEnumerable<MachineReviewDto>> GetMachineReviews(List<int> machineIds);
        Task<MachineDetailDto> GetMachineDetail(int machineId);
        Task<IEnumerable<MachineSerialNumberDto>> GetSerialMachineList(int machineId);
        Task<MachineDto> CreateMachine(CreateMachineDto createMachineDto);
        Task DeleteMachine(int machineId);
        Task UpdateMachineStatus(int machineId, string status);
        Task UpdateMachineAttribute(int machineId, IEnumerable<CreateMachineAttributeDto> productAttributeDtos);
        Task UpdateMachineDetail(int machineId, UpdateMachineDto updateMachineDto, int accountId);
        Task UpdateMachineComponent(int machineId, ComponentList productAttributeDtos);
        Task ChangeMachineImages(int machineId, List<ImageList> imageList);
        Task UpdateMachineTerm(int machineId, IEnumerable<CreateMachineTermDto> productTermDtos);
        Task ToggleLockStatus(int machineId);
        Task<MachineQuotationDto> GetMachineQuotation(int machineId);
        Task<List<MachineQuotationDto>> GetMachineQuotations();
    }
}
