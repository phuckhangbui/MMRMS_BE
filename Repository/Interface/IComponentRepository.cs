using DTOs.Component;

namespace Repository.Interface
{
    public interface IComponentRepository
    {
        Task<IEnumerable<ComponentDto>> GetAll();

        Task Create(CreateComponentDto createComponentDto);

        Task Update(ComponentDto componentDto);

        Task Delete(int componentId);

        Task<bool> IsComponentIdExisted(int componentId);
        Task<ComponentDto> Create(string name);

        Task<bool> IsComponentNameExisted(string componentName);

    }
}
