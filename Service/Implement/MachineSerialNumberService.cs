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
    public class MachineSerialNumberService : IMachineSerialNumberService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IMachineRepository _productRepository;
        private readonly IMachineSerialNumberComponentRepository _machineSerialNumberComponentRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IMapper _mapper;

        public MachineSerialNumberService(IMachineSerialNumberRepository machineSerialNumberRepository, IMachineRepository productRepository, IMapper mapper, IMachineSerialNumberComponentRepository machineSerialNumberComponentRepository, IComponentRepository componentRepository, ISettingsService settingsService)
        {
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _machineSerialNumberComponentRepository = machineSerialNumberComponentRepository;
            _componentRepository = componentRepository;
            _settingsService = settingsService;
        }

        public async Task CreateMachineSerialNumber(MachineSerialNumberCreateRequestDto dto, int accountId)
        {
            var machineDetail = await _productRepository.GetMachineDetail(dto.MachineId);

            if (machineDetail == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }

            if (await _machineSerialNumberRepository.IsSerialNumberExist(dto.SerialNumber))
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberDuplicated);
            }

            if (machineDetail.MachineComponentList.IsNullOrEmpty())
            {
                if (!dto.ForceWhenNoComponentInMachine)
                {
                    throw new ServiceException(MessageConstant.MachineSerialNumber.MachineHaveNoComponentAndIsForceSetToFalse);
                }
            }

            int rentDaysCounter = 0;
            var actualRentPrice = machineDetail.RentPrice;
            var percent = dto.MachineConditionPercent;

            if (percent != 100)
            {
                if (percent < 60)
                {
                    throw new ServiceException(MessageConstant.MachineSerialNumber.PercentHigherThanSixty);
                }

                if (percent % 5 != 0)
                {
                    throw new ServiceException(MessageConstant.MachineSerialNumber.PercentIsNotDivisibleByFive);
                }

                var machineSetting = await _settingsService.GetMachineSettingsAsync();

                foreach (var condition in machineSetting.DaysData)
                {
                    if (dto.MachineConditionPercent == condition.MachineConditionPercent)
                    {
                        rentDaysCounter = condition.RentedDays;
                        break;
                    }
                }


                foreach (var condition in machineSetting.RateData)
                {
                    if (dto.MachineConditionPercent == condition.MachineConditionPercent)
                    {
                        actualRentPrice = actualRentPrice * (condition.RentalPricePercent / 100.0);

                        actualRentPrice = NumberExtension.SquareMoneyToNearest1000((double)actualRentPrice);
                        break;
                    }
                }
            }

            var serialMachineDto = new MachineSerialNumberDto
            {
                SerialNumber = dto.SerialNumber,
                MachineId = dto.MachineId,
                MachineConditionPercent = dto.MachineConditionPercent,
                ActualRentPrice = actualRentPrice,
                RentDaysCounter = rentDaysCounter,
                Status = MachineSerialNumberStatusEnum.Available.ToString(),
                DateCreate = DateTime.Now,
            };

            await _machineSerialNumberRepository.CreateMachineSerialNumber(serialMachineDto, machineDetail.MachineComponentList, accountId);

            var product = _mapper.Map<MachineDto>(machineDetail);

            if (product.Status == MachineStatusEnum.Locked.ToString())
            {
                product.Status = MachineStatusEnum.Active.ToString();
            }

            await _productRepository.UpdateMachine(product);
        }

        public async Task Delete(string serialNumber)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);

            }


            if (await _machineSerialNumberRepository.IsMachineSerialNumberHasContract(serialNumber))
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberHasContract);
            }

            await _machineSerialNumberRepository.Delete(serialNumber);


            var serialMachineList = await _productRepository.GetMachineNumberList((int)machineSerialNumber.MachineId);


            var productDto = await _productRepository.GetMachine((int)machineSerialNumber.MachineId);

            if (productDto == null)
            {
                return;
            }

            if (serialMachineList.IsNullOrEmpty())
            {
                productDto.Status = MachineStatusEnum.Locked.ToString();

                await _productRepository.UpdateMachine(productDto);

                return;
            }

            foreach (var serialMachine in serialMachineList)
            {
                if (serialMachine.Status == MachineSerialNumberStatusEnum.Available.ToString())
                {
                    productDto.Status = MachineStatusEnum.Active.ToString();

                    await _productRepository.UpdateMachine(productDto);

                    return;
                }
            }

            await _productRepository.UpdateMachine(productDto);
        }

        public async Task<IEnumerable<MachineSerialNumberLogDto>> GetDetailLog(string serialNumber)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            return await _machineSerialNumberRepository.GetMachineSerialNumberLog(serialNumber);
        }

        public async Task<IEnumerable<MachineSerialNumberComponentDto>> GetSerialNumberComponents(string serialNumber)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            return await _machineSerialNumberRepository.GetMachineComponent(serialNumber);
        }

        public async Task MoveSerialMachineToMaintenanceStatus(string serialNumber, int staffId, string note)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            if (machineSerialNumber.Status == MachineSerialNumberStatusEnum.Maintenance.ToString()
               || machineSerialNumber.Status == MachineSerialNumberStatusEnum.Renting.ToString()
               || machineSerialNumber.Status == MachineSerialNumberStatusEnum.Reserved.ToString())
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineNotSuitableForMaintenanceStatus);
            }

            await _machineSerialNumberRepository.UpdateStatus(serialNumber, MachineSerialNumberStatusEnum.Maintenance.ToString(), staffId, note);
        }

        public async Task MoveSerialMachineToActiveStatus(string serialNumber, int staffId, string note)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            if (machineSerialNumber.Status == MachineSerialNumberStatusEnum.Available.ToString()
               || machineSerialNumber.Status == MachineSerialNumberStatusEnum.Renting.ToString()
               || machineSerialNumber.Status == MachineSerialNumberStatusEnum.Reserved.ToString())
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineNotSuitableForAvailableStatus);
            }

            var machineComponent = await _machineSerialNumberRepository.GetMachineComponent(serialNumber);

            var isUpdatableToAvailable = true;

            if (!machineComponent.IsNullOrEmpty())
            {
                isUpdatableToAvailable = machineComponent.All(c => c.Status == MachineSerialNumberComponentStatusEnum.Normal.ToString());
            }

            if (!isUpdatableToAvailable)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineComponentStillBroken);
            }

            await _machineSerialNumberRepository.UpdateStatus(serialNumber, MachineSerialNumberStatusEnum.Available.ToString(), staffId, note);
        }

        public async Task ToggleStatus(string serialNumber, int staffId)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            if (machineSerialNumber.Status == MachineSerialNumberStatusEnum.Maintenance.ToString()
               || machineSerialNumber.Status == MachineSerialNumberStatusEnum.Renting.ToString())
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineStateNotSuitableForModifyStatus);
            }

            if (machineSerialNumber.Status == MachineSerialNumberStatusEnum.Available.ToString())
            {
                machineSerialNumber.Status = MachineSerialNumberStatusEnum.Locked.ToString();
            }
            else
            {
                machineSerialNumber.Status = MachineSerialNumberStatusEnum.Available.ToString();
            }

            await _machineSerialNumberRepository.UpdateStatus(serialNumber, machineSerialNumber.Status, staffId);
        }

        public async Task UpdateMachineSerialNumberComponentStatusToBrokenWhileInStore(int machineSerialNumberComponentId, int accountId, string note)
        {
            var serialComponent = await _machineSerialNumberComponentRepository.GetComponent(machineSerialNumberComponentId);

            if (serialComponent == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
            }

            var serialMachine = await _machineSerialNumberRepository.GetMachineSerialNumber(serialComponent.SerialNumber);

            if (serialMachine.Status == MachineSerialNumberStatusEnum.Renting.ToString()
                || serialMachine.Status == MachineSerialNumberStatusEnum.Reserved.ToString())
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.YouCannotUpdateStatusOfComponentWhileRenting);
            }

            if (serialComponent.Status == MachineSerialNumberComponentStatusEnum.Broken.ToString())
            {
                return;
            }

            await _machineSerialNumberComponentRepository.UpdateComponentStatus(machineSerialNumberComponentId, MachineSerialNumberComponentStatusEnum.Broken.ToString(), accountId, note);

            if (serialMachine.Status == MachineSerialNumberStatusEnum.Available.ToString())
            {
                await _machineSerialNumberRepository.UpdateStatus(serialMachine.SerialNumber, MachineSerialNumberStatusEnum.Maintenance.ToString(), accountId);
            }
        }

        public async Task UpdateMachineSerialNumberComponentStatusToNormalWhileInStore(int machineSerialNumberComponentId,
                                                                                       int staffId,
                                                                                       bool isDeductFromComponentStorage,
                                                                                       int quantity,
                                                                                       string note)
        {
            var serialComponent = await _machineSerialNumberComponentRepository.GetComponent(machineSerialNumberComponentId);

            if (serialComponent == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
            }

            var serialMachine = await _machineSerialNumberRepository.GetMachineSerialNumber(serialComponent.SerialNumber);

            if (serialMachine.Status == MachineSerialNumberStatusEnum.Renting.ToString()
                || serialMachine.Status == MachineSerialNumberStatusEnum.Reserved.ToString())
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.YouCannotUpdateStatusOfComponentWhileRenting);
            }

            if (serialComponent.Status == MachineSerialNumberComponentStatusEnum.Normal.ToString())
            {
                return;
            }

            if (isDeductFromComponentStorage)
            {
                if (serialComponent.AvailableQuantity < quantity)
                {
                    throw new ServiceException(MessageConstant.MachineSerialNumber.InvalidQuantity);
                }

                var component = await _componentRepository.GetComponent((int)serialComponent.ComponentId);

                if (component == null)
                {
                    throw new ServiceException(MessageConstant.Component.ComponentNotExisted);
                }

                if (component.AvailableQuantity.HasValue && component.AvailableQuantity < quantity)
                {
                    throw new ServiceException(MessageConstant.ComponentReplacementTicket.NotEnoughQuantity);
                }

                component.AvailableQuantity -= quantity;
                if (component.AvailableQuantity == 0)
                {
                    component.Status = ComponentStatusEnum.OutOfStock.ToString();
                }

                await _componentRepository.UpdateComponent(component);
            }

            await _machineSerialNumberComponentRepository.UpdateComponentStatus(machineSerialNumberComponentId, MachineSerialNumberComponentStatusEnum.Normal.ToString(), staffId, note);

            //no need to update machine serial status, staff must have to update this manually

            //if (serialMachine.Status == MachineSerialNumberStatusEnum.Available.ToString())
            //{
            //    await _machineSerialNumberRepository.UpdateStatus(serialMachine.SerialNumber, MachineSerialNumberStatusEnum.Maintenance.ToString(), staffId);
            //}
        }

        public async Task<MachineSerialNumberDto> GetMachineSerial(string serialNumber)
        {
            return await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);
        }

        public async Task UpdateRentDaysCounterMachineSerialNumber(string serialNumber, int actualRentPeriod)
        {

            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            // Update RentDaysCounter
            machineSerialNumber.RentDaysCounter = (machineSerialNumber.RentDaysCounter ?? 0) + actualRentPeriod;

            // Retrieve machine settings
            var machineSetting = await _settingsService.GetMachineSettingsAsync();

            // Determine MachineConditionPercent based on RentDaysCounter
            var condition = machineSetting.DaysData
                .OrderByDescending(d => d.RentedDays)
                .FirstOrDefault(d => machineSerialNumber.RentDaysCounter >= d.RentedDays);

            if (condition != null)
            {
                machineSerialNumber.MachineConditionPercent = condition.MachineConditionPercent;
            }

            // Determine ActualRentPrice based on MachineConditionPercent
            if (machineSerialNumber.MachineConditionPercent.HasValue)
            {
                var rate = machineSetting.RateData
                    .OrderByDescending(r => r.MachineConditionPercent)
                    .FirstOrDefault(r => machineSerialNumber.MachineConditionPercent >= r.MachineConditionPercent);

                var machine = await _productRepository.GetMachine((int)machineSerialNumber.MachineId);

                if (rate != null)
                {
                    machineSerialNumber.ActualRentPrice = machine.RentPrice * (rate.RentalPricePercent / 100.0);
                }
            }

            //await _machineSerialNumberRepository.UpdateMachineSerialNumber();
        }
    }
}
