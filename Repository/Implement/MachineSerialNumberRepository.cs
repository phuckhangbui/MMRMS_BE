using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.Machine;
using DTOs.MachineSerialNumber;
using DTOs.RentingRequest;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class MachineSerialNumberRepository : IMachineSerialNumberRepository
    {
        private readonly IMapper _mapper;

        public MachineSerialNumberRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CheckMachineSerialNumbersValidToRent(List<MachineSerialNumberRentRequestDto> machineSerialNumberRentRequestDtos)
        {
            foreach (var machineSerialNumberRentRequestDto in machineSerialNumberRentRequestDtos)
            {
                var isMachineSerialNumberValid = await MachineSerialNumberDao.Instance.IsMachineSerialNumberValidToRent(
                    machineSerialNumberRentRequestDto.SerialNumber, machineSerialNumberRentRequestDto.MachineId);

                if (!isMachineSerialNumberValid)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CheckMachineSerialNumberValidToRequest(NewRentingRequestDto newRentingRequestDto)
        {
            if (newRentingRequestDto.RentingRequestMachineDetails.IsNullOrEmpty())
            {
                return false;
            }

            foreach (var rentingRequestMachineDetailDto in newRentingRequestDto.RentingRequestMachineDetails)
            {
                var isMachineSerialNumberValid = await MachineSerialNumberDao.Instance
                    .IsMachineSerialNumberValidToRent(
                            rentingRequestMachineDetailDto.MachineId,
                            rentingRequestMachineDetailDto.Quantity,
                            newRentingRequestDto.DateStart,
                            newRentingRequestDto.DateEnd);

                if (!isMachineSerialNumberValid)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task CreateMachineSerialNumber(MachineSerialNumberCreateRequestDto createSerialMachineNumberDto, IEnumerable<MachineComponentDto> componentMachineList, double price, int accountId)
        {
            var now = DateTime.Now;

            var serialMachine = new MachineSerialNumber
            {
                SerialNumber = createSerialMachineNumberDto.SerialNumber,
                MachineId = createSerialMachineNumberDto.MachineId,
                DateCreate = now,
                RentDaysCounter = 0,
                ActualRentPrice = price,
                Status = MachineSerialNumberStatusEnum.Available.ToString()
            };

            IList<MachineComponentStatus> productComponentStatuses = new List<MachineComponentStatus>();

            foreach (var componentMachine in componentMachineList)
            {
                var productComponentStatus = new MachineComponentStatus
                {
                    SerialNumber = createSerialMachineNumberDto.SerialNumber,
                    ComponentId = componentMachine.MachineComponentId,
                    Quantity = componentMachine.Quantity,
                    Status = MachineComponentStatusEnum.Normal.ToString()
                };

                productComponentStatuses.Add(productComponentStatus);
            }

            if (productComponentStatuses.Count == 0)
            {
                serialMachine.MachineComponentStatuses = null;
            }
            else
            {
                serialMachine.MachineComponentStatuses = productComponentStatuses;
            }

            MachineSerialNumberLog log = new MachineSerialNumberLog
            {
                SerialNumber = serialMachine.SerialNumber,
                AccountTriggerId = accountId,
                Action = "Create new serial number product",
                Type = MachineSerialNumberLogTypeEnum.System.ToString(),
                DateCreate = now
            };

            serialMachine.MachineSerialNumberLogs = [log];

            await MachineSerialNumberDao.Instance.CreateAsync(serialMachine);

        }

        public async Task Delete(string serialNumber)
        {
            await MachineSerialNumberDao.Instance.Delete(serialNumber);
        }

        public async Task<IEnumerable<MachineComponentStatusDto>> GetMachineComponentStatus(string serialNumber)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumberDetail(serialNumber);

            if (machineSerialNumber.MachineComponentStatuses.IsNullOrEmpty())
            {
                return new List<MachineComponentStatusDto>();
            }

            return _mapper.Map<IEnumerable<MachineComponentStatusDto>>(machineSerialNumber.MachineComponentStatuses);
        }

        public async Task<MachineSerialNumberDto> GetMachineSerialNumber(string serialNumber)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(serialNumber);

            return _mapper.Map<MachineSerialNumberDto>(machineSerialNumber);
        }

        public async Task<IEnumerable<MachineSerialNumberLogDto>> GetMachineSerialNumberLog(string serialNumber)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumberDetail(serialNumber);

            if (machineSerialNumber.MachineSerialNumberLogs.IsNullOrEmpty())
            {
                return new List<MachineSerialNumberLogDto>();
            }

            return _mapper.Map<List<MachineSerialNumberLogDto>>(machineSerialNumber.MachineSerialNumberLogs);
        }

        public async Task<IEnumerable<MachineSerialNumberOptionDto>> GetSerialMachineNumbersAvailableForRenting(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(rentingRequestId);

            var allMachineSerialNumbers = new List<MachineSerialNumber>();

            foreach (var rentingRequestMachineDetail in rentingRequest.RentingRequestMachineDetails)
            {
                var machineSerialNumbers = await MachineSerialNumberDao.Instance
                    .GetMachineSerialNumbersByMachineIdAndStatus((int)rentingRequestMachineDetail.MachineId!, MachineSerialNumberStatusEnum.Available.ToString());

                allMachineSerialNumbers.AddRange(machineSerialNumbers);
            }

            return _mapper.Map<IEnumerable<MachineSerialNumberOptionDto>>(allMachineSerialNumbers);
        }

        public async Task<bool> IsSerialNumberExist(string serialNumber)
        {
            return await MachineSerialNumberDao.Instance.IsSerialNumberExisted(serialNumber);
        }

        public async Task<bool> IsMachineSerialNumberHasContract(string serialNumber)
        {
            return await MachineSerialNumberDao.Instance.IsSerialNumberInAnyContract(serialNumber);
        }

        public async Task Update(string serialNumber, MachineSerialNumberUpdateDto machineSerialNumberUpdateDto)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber != null)
            {
                machineSerialNumber.ActualRentPrice = machineSerialNumberUpdateDto.ActualRentPrice;
                machineSerialNumber.RentDaysCounter = machineSerialNumberUpdateDto.RentDaysCounter;

                await MachineSerialNumberDao.Instance.UpdateAsync(machineSerialNumber);
            }
        }

        public async Task UpdateStatus(string serialNumber, string status, int accountId)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(serialNumber);

            machineSerialNumber.Status = status;

            var log = new MachineSerialNumberLog
            {
                SerialNumber = serialNumber,
                AccountTriggerId = accountId,
                Type = MachineSerialNumberLogTypeEnum.UpdateStatus.ToString(),
                DateCreate = DateTime.Now,
                Action = $"Change status to {status}"
            };

            await MachineSerialNumberDao.Instance.UpdateAsync(machineSerialNumber);

            await MachineSerialNumberLogDao.Instance.CreateAsync(log);
        }
    }
}
