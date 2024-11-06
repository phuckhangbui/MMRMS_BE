using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.Component;
using DTOs.Machine;
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

        public async Task<IEnumerable<MachineComponentDto>> GetMachineComponentList(int componentId)
        {
            var list = await MachineComponentDao.Instance.GetMachineComponentBaseOnComponentId(componentId);

            return _mapper.Map<IEnumerable<MachineComponentDto>>(list);
        }

        public async Task UpdateComponent(ComponentDto componentDto)
        {
            var component = await ComponentDao.Instance.GetComponent(componentDto.ComponentId);

            component.ComponentName = componentDto.ComponentName;
            component.Status = componentDto.Status;
            component.Price = componentDto.Price;
            component.AvailableQuantity = componentDto.AvailableQuantity;

            await ComponentDao.Instance.UpdateAsync(component);
        }

        public async Task MoveComponentQuanityFromAvailableToOnHold(int componentId, int quantity)
        {
            var component = await ComponentDao.Instance.GetComponent(componentId);

            if (component == null)
            {
                throw new Exception(MessageConstant.Component.ComponentNotExisted);
            }

            component.AvailableQuantity -= quantity;
            if (component.QuantityOnHold == null)
            {
                component.QuantityOnHold = 0;
            }
            component.QuantityOnHold += quantity;

            if (component.AvailableQuantity < 0)
            {
                throw new Exception(MessageConstant.ComponentReplacementTicket.NotEnoughQuantity);
            }

            if (component.AvailableQuantity == 0)
            {
                component.Status = ComponentStatusEnum.OutOfStock.ToString();
            }

            await ComponentDao.Instance.UpdateAsync(component);
        }

        public async Task MoveComponentQuanityFromOnHoldToAvailable(int componentId, int quantity)
        {
            var component = await ComponentDao.Instance.GetComponent(componentId);

            if (component == null)
            {
                throw new Exception(MessageConstant.Component.ComponentNotExisted);
            }

            component.AvailableQuantity += quantity;
            if (component.QuantityOnHold == null)
            {
                component.QuantityOnHold = 0;
            }
            component.QuantityOnHold -= quantity;

            if (component.Status == ComponentStatusEnum.OutOfStock.ToString())
            {
                component.Status = ComponentStatusEnum.Active.ToString();
            }

            await ComponentDao.Instance.UpdateAsync(component);
        }

        public async Task RemoveOnHoldQuantity(int componentId, int quantity)
        {
            var component = await ComponentDao.Instance.GetComponent(componentId);

            if (component == null)
            {
                throw new Exception(MessageConstant.Component.ComponentNotExisted);
            }

            component.QuantityOnHold -= quantity;

            await ComponentDao.Instance.UpdateAsync(component);
        }
    }
}
