using AutoMapper;
using BusinessObject;
using Common;
using DAO;
using DTOs.Component;
using Repository.Exceptions;
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
            var isExisted = await ComponentDao.Instance.IsComponentNameExisted(createComponentDto.ComponentName.Trim());

            if (isExisted)
            {
                throw new RepositoryException(MessageConstant.Component.ComponetNameDuplicated);
            }

            var component = _mapper.Map<Component>(createComponentDto);

            component.ComponentName = createComponentDto.ComponentName.Trim();
            component.DateCreate = DateTime.Now;
            component.Status = "Active";

            await ComponentDao.Instance.CreateAsync(component);
        }

        public async Task<ComponentDto> Create(string name)
        {
            var isExisted = await ComponentDao.Instance.IsComponentNameExisted(name.Trim());

            if (isExisted)
            {
                throw new RepositoryException(MessageConstant.Component.ComponetNameDuplicated);
            }

            var component = new Component
            {
                ComponentName = name.Trim(),
                Price = null,
                Quantity = null,
                DateCreate = DateTime.Now,
                Status = "NoPriceAndQuantity"
            };

            component = await ComponentDao.Instance.CreateComponent(component);

            return _mapper.Map<ComponentDto>(component);
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

        public async Task<bool> IsComponentIdExisted(int componentId)
        {
            return await ComponentDao.Instance.IsComponentIdExisted(componentId);
        }

        public async Task<bool> IsComponentNameExisted(string componentName)
        {
            return await ComponentDao.Instance.IsComponentNameExisted(componentName);
        }

    }
}
