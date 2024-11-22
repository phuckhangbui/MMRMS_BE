using AutoMapper;
using Common;
using Common.Enum;
using DTOs.Machine;
using DTOs.MachineSerialNumber;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class MachineServiceImpl : IMachineService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public MachineServiceImpl(IMachineRepository machineRepository, IComponentRepository componentRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _machineRepository = machineRepository;
            _componentRepository = componentRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MachineViewDto>> GetMachineList()
        {
            return await _machineRepository.GetMachineList();
        }

        public async Task<IEnumerable<MachineDto>> GetActiveMachines()
        {
            return await _machineRepository.GetActiveMachines();
        }

        public async Task<IEnumerable<MachineDto>> GetTop8LatestMachineList()
        {
            return await _machineRepository.GetTop8LatestMachineList();
        }

        public async Task<MachineDetailDto> GetMachineDetail(int machineId)
        {
            var machine = await _machineRepository.GetMachineDetail(machineId);

            if (machine == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }

            return machine;
        }

        public async Task<IEnumerable<MachineSerialNumberDto>> GetSerialMachineList(int productId)
        {
            var isMachineExisted = await _machineRepository.IsMachineExisted(productId);

            if (isMachineExisted)
            {
                var list = await _machineRepository.GetMachineNumberList(productId);
                return _mapper.Map<IEnumerable<MachineSerialNumberDto>>(list);
            }

            throw new ServiceException(MessageConstant.Machine.MachineNotFound);

        }

        public async Task<MachineDto> CreateMachine(CreateMachineDto createMachineDto)
        {
            if (await _machineRepository.IsMachineNameExisted(createMachineDto.MachineName))
            {
                throw new ServiceException(MessageConstant.Machine.MachineNameDuplicated);
            }

            if (await _machineRepository.IsMachineModelExisted(createMachineDto.Model))
            {
                throw new ServiceException(MessageConstant.Machine.MachineModelDuplicated);
            }

            var category = await _categoryRepository.GetCategoryById(createMachineDto.CategoryId);

            if (category == null)
            {
                throw new ServiceException(MessageConstant.Category.CategoryNotFound);
            }

            var flag = true;


            if (!createMachineDto.ExistedComponentList.IsNullOrEmpty())
            {
                foreach (var component in createMachineDto.ExistedComponentList)
                {
                    if (!await _componentRepository.IsComponentIdExisted(component.ComponentId))
                    {
                        flag = false;
                    }
                }
            }

            if (!flag)
            {
                throw new ServiceException(MessageConstant.Machine.ComponentIdListNotCorrect);
            }

            if (!createMachineDto.NewComponentList.IsNullOrEmpty())
            {
                foreach (var component in createMachineDto.NewComponentList)
                {
                    if (await _componentRepository.IsComponentNameExisted(component.ComponentName))
                    {
                        flag = false;
                    }
                }
            }

            if (!flag)
            {
                throw new ServiceException(MessageConstant.Component.ComponetNameDuplicated);
            }

            if (createMachineDto.ImageUrls.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Machine.ImageIsRequired);
            }

            if (createMachineDto.ImageUrls.Count() < 1)
            {
                throw new ServiceException(MessageConstant.Machine.ImageIsRequired);
            }

            var productDto = await _machineRepository.CreateMachine(createMachineDto);

            return productDto;
        }

        public async Task DeleteMachine(int productId)
        {
            var productDto = await _machineRepository.GetMachine(productId);

            var productNumberList = await _machineRepository.GetMachineNumberList(productId);

            if (productDto == null)
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);

            if (!productNumberList.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Machine.MachineHasSerialNumberCannotDeleted);

            }

            await _machineRepository.DeleteMachine(productId);
            //else productDto.IsDelete = true;

            //await _productRepository.UpdateMachine(productDto);
        }

        public async Task UpdateMachineStatus(int productId, string status)
        {
            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(MachineStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.Machine.StatusNotAvailable);
            }

            var productDto = await _machineRepository.GetMachine(productId);

            if (productDto == null)
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);

            productDto.Status = status;

            await _machineRepository.UpdateMachine(productDto);
        }

        public async Task UpdateMachineDetail(int productId, UpdateMachineDto updateMachineDto)
        {
            var productDto = await _machineRepository.GetMachine(productId);
            if (productDto == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }

            if (!productDto.MachineName.ToLower().Equals(updateMachineDto.MachineName.ToLower()))
            {
                if (await _machineRepository.IsMachineNameExisted(updateMachineDto.MachineName))
                {
                    throw new ServiceException(MessageConstant.Machine.MachineNameDuplicated);
                }
            }
            if (!productDto.Model.ToLower().Equals(updateMachineDto.Model.ToLower()))
            {
                if (await _machineRepository.IsMachineModelExisted(updateMachineDto.Model))
                {
                    throw new ServiceException(MessageConstant.Machine.MachineModelDuplicated);
                }
            }

            var category = await _categoryRepository.GetCategoryById(updateMachineDto.CategoryId);

            if (category == null)
            {
                throw new ServiceException(MessageConstant.Category.CategoryNotFound);

            }

            productDto.MachineName = updateMachineDto.MachineName;
            productDto.Description = updateMachineDto.Description;
            productDto.RentPrice = updateMachineDto.RentPrice;
            productDto.MachinePrice = updateMachineDto.MachinePrice;
            productDto.Model = updateMachineDto.Model;
            productDto.Origin = updateMachineDto.Origin;
            productDto.CategoryId = updateMachineDto.CategoryId;

            await _machineRepository.UpdateMachine(productDto);
        }

        public async Task UpdateMachineAttribute(int productId, IEnumerable<CreateMachineAttributeDto> productAttributeDtos)
        {
            var product = await _machineRepository.GetMachine(productId);

            if (product == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }

            await _machineRepository.UpdateMachineAttribute(productId, productAttributeDtos);
        }

        public async Task UpdateMachineComponent(int productId, ComponentList componentList)
        {
            var product = await _machineRepository.GetMachine(productId);

            if (product == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }

            var serialMachines = await _machineRepository.GetMachineNumberList(productId);

            if (!serialMachines.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Machine.MachineHasSerialNumberCannotUpdateComponentList);
            }

            if (!componentList.ExistedComponentList.IsNullOrEmpty())
                foreach (var component in componentList.ExistedComponentList)
                {
                    if (!await _componentRepository.IsComponentIdExisted(component.ComponentId))
                    {
                        throw new ServiceException(MessageConstant.Component.ComponentNotExisted);
                    }
                }
            if (!componentList.NewComponentList.IsNullOrEmpty())
                foreach (var component in componentList.NewComponentList)
                {
                    if (await _componentRepository.IsComponentNameExisted(component.ComponentName))
                    {
                        throw new ServiceException(MessageConstant.Component.ComponetNameDuplicated);
                    }
                }

            await _machineRepository.UpdateMachineComponent(productId, componentList);
        }


        public async Task ChangeMachineImages(int productId, List<ImageList> imageList)
        {
            var product = await _machineRepository.GetMachine(productId);

            if (product == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }


            if (imageList.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Machine.ImageIsRequired);
            }

            await _machineRepository.UpdateMachineImage(productId, imageList);

        }

        public async Task UpdateMachineTerm(int productId, IEnumerable<CreateMachineTermDto> productTermDtos)
        {
            var product = await _machineRepository.GetMachine(productId);

            if (product == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }

            await _machineRepository.UpdateMachineTerm(productId, productTermDtos);
        }

        public async Task<IEnumerable<MachineReviewDto>> GetMachineReviews(List<int> productIds)
        {
            return await _machineRepository.GetMachineReviews(productIds);
        }

        public async Task ToggleLockStatus(int productId)
        {

            var productDto = await _machineRepository.GetMachine(productId);

            if (productDto == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }

            if (productDto.Status != MachineStatusEnum.Locked.ToString())
            {
                productDto.Status = MachineStatusEnum.Locked.ToString();

                await _machineRepository.UpdateMachine(productDto);

                return;
            }

            var serialMachineList = await GetSerialMachineList(productId);

            if (serialMachineList.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Machine.MachineStateNotSuitableForModifyStatus);
            }

            foreach (var serialMachine in serialMachineList)
            {
                if (serialMachine.Status == MachineSerialNumberStatusEnum.Available.ToString())
                {
                    productDto.Status = MachineStatusEnum.Active.ToString();

                    await _machineRepository.UpdateMachine(productDto);

                    return;
                }
            }
        }
    }
}
