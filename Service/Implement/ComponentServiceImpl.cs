using DTOs.Component;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class ComponentServiceImpl : IComponentService
    {
        private readonly IComponentRepository _componentRepository;

        public ComponentServiceImpl(IComponentRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }


        public async Task<IEnumerable<ComponentDto>> GetComponents()
        {
            return await _componentRepository.GetAll();
        }
        public async Task CreateComponet(CreateComponentDto createComponentDto)
        {
            await _componentRepository.Create(createComponentDto);
        }

    }
}
