using DTOs.Component;

namespace Service.Interface
{
    public interface IComponentService
    {
        Task<IEnumerable<ComponentDto>> GetComponents();
        Task CreateComponent(CreateComponentDto createComponentDto);
        Task UpdateComponent(UpdateComponentDto updateComponentDto);

        Task UpdateComponentStatus(int componentId, string status);
        Task DeleteComponent(int componentId);
    }
}
