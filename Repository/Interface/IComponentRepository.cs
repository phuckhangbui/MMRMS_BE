using DTOs.Component;
using DTOs.Machine;

namespace Repository.Interface
{
    public interface IComponentRepository
    {
        Task<IEnumerable<ComponentDto>> GetAll();

        Task Create(CreateComponentDto createComponentDto);

        Task Delete(int componentId);

        Task<bool> IsComponentIdExisted(int componentId);
        //Task<ComponentDto> Create(string name);

        Task<bool> IsComponentNameExisted(string componentName);
        Task<ComponentDto> GetComponent(int componentId);
        Task<IEnumerable<MachineComponentDto>> GetMachineComponentList(int componentId);
        Task UpdateComponent(ComponentDto component);

        Task MoveComponentQuanityFromAvailableToOnHold(int componentId, int quantity);
        Task MoveComponentQuanityFromOnHoldToAvailable(int componentId, int quantity);
        Task RemoveOnHoldQuantity(int componentId, int quantity);
    }
}
