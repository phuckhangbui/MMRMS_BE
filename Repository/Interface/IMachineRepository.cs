using DTOs.Machine;
using DTOs.MachineSerialNumber;

namespace Repository.Interface
{
    public interface IMachineRepository
    {
        Task<IEnumerable<MachineViewDto>> GetMachineList();
        Task<IEnumerable<MachineDto>> GetActiveMachines();
        Task<IEnumerable<MachineDto>> GetTop8LatestMachineList();
        Task<bool> IsMachineExisted(int productId);
        Task<bool> IsMachineNameExisted(string name);
        Task<bool> IsMachineModelExisted(string model);
        Task<MachineDetailDto?> GetMachineDetail(int machineId);
        Task<IEnumerable<MachineSerialNumberDto>> GetMachineNumberList(int productId);
        Task<MachineDto> CreateMachine(CreateMachineDto createMachineDto);
        Task<MachineDto> GetMachine(int productId);
        Task UpdateMachine(MachineDto productDto);
        Task DeleteMachine(int productId);
        Task UpdateMachineAttribute(int productId, IEnumerable<CreateMachineAttributeDto> productAttributeDtos);
        Task UpdateMachineComponent(int productId, ComponentList productComponentDtos);
        Task ChangeMachineThumbnail(int productId, string imageUrlStr);
        Task UpdateMachineImage(int productId, List<ImageList> imageList);
        Task UpdateMachineTerm(int productId, IEnumerable<CreateMachineTermDto> productTermDtos);
        Task<IEnumerable<MachineReviewDto>> GetMachineReviews(List<int> machineIds);
    }
}
