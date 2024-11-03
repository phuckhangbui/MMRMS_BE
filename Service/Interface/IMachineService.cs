using DTOs.Machine;
using DTOs.MachineSerialNumber;

namespace Service.Interface
{
    public interface IMachineService
    {
        Task<IEnumerable<MachineViewDto>> GetMachineList();
        Task<IEnumerable<MachineDto>> GetActiveMachines();
        Task<IEnumerable<MachineDto>> GetTop8LatestMachineList();
        Task<IEnumerable<MachineReviewDto>> GetMachineReviews(List<int> productIds);
        Task<MachineDetailDto> GetMachineDetail(int machineId);
        Task<IEnumerable<MachineSerialNumberDto>> GetSerialMachineList(int productId);
        Task<MachineDto> CreateMachine(CreateMachineDto createMachineDto);
        Task DeleteMachine(int productId);
        Task UpdateMachineStatus(int productId, string status);
        Task UpdateMachineAttribute(int productId, IEnumerable<CreateMachineAttributeDto> productAttributeDtos);
        Task UpdateMachineDetail(int productId, UpdateMachineDto updateMachineDto);
        Task UpdateMachineComponent(int productId, ComponentList productAttributeDtos);
        Task ChangeMachineImages(int productId, List<ImageList> imageList);
        Task UpdateMachineTerm(int productId, IEnumerable<CreateMachineTermDto> productTermDtos);
        Task ToggleLockStatus(int productId);
    }
}
