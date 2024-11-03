using DTOs.Machine;
using DTOs.MachineComponentStatus;

namespace Service.Interface
{
    public interface IMachineService
    {
        Task<IEnumerable<MachineDto>> GetMachineList();
        Task<IEnumerable<MachineDto>> GetTop8LatestMachineList();
        Task<IEnumerable<MachineReviewDto>> GetMachineReviews(List<int> productIds);
        Task<DisplayMachineDetailDto> GetMachineDetailDto(int productId);
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
