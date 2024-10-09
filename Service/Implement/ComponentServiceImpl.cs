using Common;
using Common.Enum;
using DTOs.Component;
using Microsoft.IdentityModel.Tokens;
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

        public async Task UpdateComponent(UpdateComponentDto updateComponentDto)
        {
            var component = await _componentRepository.GetComponent(updateComponentDto.ComponentId);

            if (component == null)
            {
                throw new ServiceException(MessageConstant.Component.ComponentNotExisted);
            }

            if (updateComponentDto.ComponentName.Trim().ToLower() != component.ComponentName.Trim().ToLower())
            {

                var componentProducts = await _componentRepository.GetComponentProductList(component.ComponentId);

                if (!componentProducts.IsNullOrEmpty())
                {
                    throw new ServiceException(MessageConstant.Component.ComponentHasBeenUsedCannotUpdateName);
                }
            }


            if (component.Quantity == null && component.Status.Equals(ComponentStatusEnum.NoQuantity.ToString()))
            {
                component.Status = ComponentStatusEnum.Active.ToString();
            }

            component.Price = updateComponentDto.Price;
            component.Quantity = updateComponentDto.Quantity;
            component.ComponentName = updateComponentDto.ComponentName;


            if (updateComponentDto.Quantity == 0)
            {
                component.Status = ComponentStatusEnum.OutOfStock.ToString();
            }

            await _componentRepository.UpdateComponent(component);
        }

        public async Task UpdateComponentStatus(int componentId, string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                throw new ServiceException(MessageConstant.Component.ComponentStatusRequired);
            }

            var component = await _componentRepository.GetComponent(componentId);

            if (component == null)
            {
                throw new ServiceException(MessageConstant.Component.ComponentNotExisted);
            }


            if (!status.Equals(ComponentStatusEnum.Active.ToString()) &&
                !status.Equals(ComponentStatusEnum.Locked.ToString()))
            {
                throw new ServiceException(MessageConstant.Component.ComponentStatusCannotSet);
            }

            component.Status = status;

            await _componentRepository.UpdateComponent(component);
        }

        public async Task DeleteComponent(int componentId)
        {
            var component = await _componentRepository.GetComponent(componentId);

            if (component == null)
            {
                throw new ServiceException(MessageConstant.Component.ComponentNotExisted);
            }

            var componentProducts = await _componentRepository.GetComponentProductList(componentId);

            if (!componentProducts.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Component.ComponentHasBeenUsedCannotDelete);
            }

            await _componentRepository.Delete(componentId);
        }
    }
}
