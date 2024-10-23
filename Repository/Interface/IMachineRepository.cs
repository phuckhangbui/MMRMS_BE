using DTOs.Machine;
using DTOs.MachineSerialNumber;

namespace Repository.Interface
{
    public interface IMachineRepository
    {
        Task<IEnumerable<MachineDto>> GetMachineList();
        Task<IEnumerable<MachineDto>> GetTop8LatestMachineList();
        Task<bool> IsMachineExisted(int productId);
        Task<bool> IsMachineExisted(string name);
        Task<bool> IsMachineModelExisted(string model);
        Task<DisplayMachineDetailDto> GetMachineDetail(int productId);
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
        Task<IEnumerable<MachineReviewDto>> GetMachineReviews(List<int> productIds);
    }
}
