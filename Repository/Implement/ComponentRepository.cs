using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.Component;
using DTOs.Product;
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
            component.Status = ComponentStatusEnum.Active.ToString();

            await ComponentDao.Instance.CreateAsync(component);
        }

        //public async Task<ComponentDto> Create(string name)
        //{
        //    var isExisted = await ComponentDao.Instance.IsComponentNameExisted(name.Trim());

        //    if (isExisted)
        //    {
        //        throw new RepositoryException(MessageConstant.Component.ComponetNameDuplicated);
        //    }

        //    var component = new Component
        //    {
        //        ComponentName = name.Trim(),
        //        Price = null,
        //        Quantity = null,
        //        DateCreate = DateTime.Now,
        //        Status = ComponentStatusEnum.NoQuantity.ToString(),
        //    };

        //    component = await ComponentDao.Instance.CreateComponent(component);

        //    return _mapper.Map<ComponentDto>(component);
        //}

        public async Task Delete(int componentId)
        {
            var componet = await ComponentDao.Instance.GetComponent(componentId);

            if (componet != null)
            {
                await ComponentDao.Instance.RemoveAsync(componet);
            }
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

        public async Task<ComponentDto> GetComponent(int componentId)
        {
            var component = await ComponentDao.Instance.GetComponent(componentId);

            return _mapper.Map<ComponentDto>(component);
        }

        public async Task<IEnumerable<ComponentProductDto>> GetComponentProductList(int componentId)
        {
            var list = await ComponentProductDao.Instance.GetComponentProductBaseOnComponentId(componentId);

            return _mapper.Map<IEnumerable<ComponentProductDto>>(list);
        }

        public async Task UpdateComponent(ComponentDto componentDto)
        {
            var component = await ComponentDao.Instance.GetComponent(componentDto.ComponentId);

            component.ComponentName = componentDto.ComponentName;
            component.Status = componentDto.Status;
            component.Price = componentDto.Price;
            component.Quantity = componentDto.Quantity;

            await ComponentDao.Instance.UpdateAsync(component);
        }
    }
}
