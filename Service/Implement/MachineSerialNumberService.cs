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
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IMachineRepository _productRepository;
        private readonly IMapper _mapper;

        public MachineSerialNumberService(IMachineSerialNumberRepository machineSerialNumberRepository, IMachineRepository productRepository, IMapper mapper)
        {
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task CreateMachineSerialNumber(MachineSerialNumberCreateRequestDto dto, int accountId)
        {
            var productDetail = await _productRepository.GetMachineDetail(dto.MachineId);

            if (productDetail == null)
            {
                throw new ServiceException(MessageConstant.Machine.MachineNotFound);
            }

            var serialMachineList = await _productRepository.GetMachineNumberList(dto.MachineId);

            if (await _machineSerialNumberRepository.IsSerialNumberExist(dto.SerialNumber))
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberDuplicated);
            }

            if (productDetail.MachineComponentList.IsNullOrEmpty())
            {
                if (!dto.ForceWhenNoComponentInMachine)
                {
                    throw new ServiceException(MessageConstant.MachineSerialNumber.MachineHaveNoComponentAndIsForceSetToFalse);
                }
            }

            await _machineSerialNumberRepository.CreateMachineSerialNumber(dto, productDetail.MachineComponentList, (double)productDetail.RentPrice, accountId);

            var product = _mapper.Map<MachineDto>(productDetail);

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

            productDto.Status = MachineStatusEnum.OutOfStock.ToString();

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

        public async Task<IEnumerable<MachineComponentStatusDto>> GetSerialNumberComponentStatus(string serialNumber)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            return await _machineSerialNumberRepository.GetMachineComponentStatus(serialNumber);
        }

        public async Task<IEnumerable<MachineSerialNumberOptionDto>> GetSerialMachineNumbersAvailableForRenting(string rentingRequestId)
        {
            var machineSerialNumbers = await _machineSerialNumberRepository.GetSerialMachineNumbersAvailableForRenting(rentingRequestId);

            if (machineSerialNumbers.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.NoAvailableSerailNumberMachineForRenting);
            }

            return machineSerialNumbers;
        }

        public async Task ToggleStatus(string serialNumber, int staffId)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber == null)
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            if (machineSerialNumber.Status == MachineSerialNumberStatusEnum.Maintenance.ToString()
               || machineSerialNumber.Status == MachineSerialNumberStatusEnum.Rented.ToString())
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

        public async Task Update(string serialNumber, MachineSerialNumberUpdateDto machineSerialNumberUpdateDto)
        {
            if (string.IsNullOrEmpty(serialNumber))
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.SerialNumberRequired);
            }

            if (!await _machineSerialNumberRepository.IsSerialNumberExist(serialNumber))
            {
                throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            await _machineSerialNumberRepository.Update(serialNumber, machineSerialNumberUpdateDto);
        }

        //public async Task UpdateStatus(string serialNumber, string status)
        //{
        //    if (string.IsNullOrEmpty(serialNumber))
        //    {
        //        throw new ServiceException(MessageConstant.MachineSerialNumber.SerialNumberRequired);
        //    }

        //    if (!await _machineSerialNumberRepository.IsSerialNumberExist(serialNumber))
        //    {
        //        throw new ServiceException(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
        //    }

        //    if (!status.Equals(MachineSerialNumberStatusEnum.Maintenance.ToString()) ||
        //       !status.Equals(MachineSerialNumberStatusEnum.Locked.ToString()) ||
        //       !status.Equals(MachineSerialNumberStatusEnum.Available.ToString()))
        //    {
        //        throw new ServiceException(MessageConstant.MachineSerialNumber.StatusCannotSet);
        //    }

        //    await _machineSerialNumberRepository.UpdateStatus(serialNumber, status, staffId);
        //}
    }
}
