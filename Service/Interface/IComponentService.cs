using DTOs.Component;

namespace Service.Interface
{
    public interface IComponentService
    {
        Task<IEnumerable<ComponentDto>> GetComponents();
        Task CreateComponent(CreateComponentDto createComponentDto);
    }
}
