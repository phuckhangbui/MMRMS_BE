using DTOs.Component;
using Repository.Exceptions;
using Repository.Interface;
using Service.Exceptions;
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
        public async Task CreateComponent(CreateComponentDto createComponentDto)
        {
            try
            {
                await _componentRepository.Create(createComponentDto);
            }
            catch (RepositoryException ex)
            { throw new ServiceException(ex.Message); };
        }

    }
}
