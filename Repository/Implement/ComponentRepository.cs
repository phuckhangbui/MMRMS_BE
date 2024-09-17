using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.Component;
using Repository.Interface;

namespace Repository.Implement
{
    public class ComponentRepository : IComponentRepository
    {

        private readonly IMapper _mapper;

        public ComponentRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task Create(CreateComponentDto createComponentDto)
        {
            var component = _mapper.Map<Component>(createComponentDto);

            component.DateCreate = DateTime.Now;
            component.Status = "Active";

            await ComponentDao.Instance.CreateAsync(component);
        }

        public Task Delete(int componentId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ComponentDto>> GetAll()
        {
            var list = await ComponentDao.Instance.GetAllAsync();

            return _mapper.Map<IEnumerable<ComponentDto>>(list);
        }

        public Task Update(ComponentDto componentDto)
        {
            throw new NotImplementedException();
        }
    }
}
