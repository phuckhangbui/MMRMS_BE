using AutoMapper;
using BusinessObject;
using Common;
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

        public async Task<List<MachineSerialNumberDto>> GetMachineSerialNumberAvailablesToRent(int machineId, DateTime startDate, DateTime endDate)
        {
            var result = await MachineSerialNumberDao.Instance.GetMachineSerialNumberAvailablesToRent(machineId, startDate, endDate);

            return _mapper.Map<List<MachineSerialNumberDto>>(result);
        }

        public async Task<List<MachineSerialNumberDto>> GetMachineSerialNumberAvailablesToRent(List<int> machineIds, DateTime startDate, DateTime endDate)
        {
            var result = await MachineSerialNumberDao.Instance.GetMachineSerialNumberAvailablesToRent(machineIds, startDate, endDate);

            return _mapper.Map<List<MachineSerialNumberDto>>(result);
        }

        public async Task<bool> CheckMachineSerialNumberValidToRent(List<RentingRequestSerialNumberDto> rentingRequestSerialNumbers)
        {
            if (rentingRequestSerialNumbers.IsNullOrEmpty())
            {
                return false;
            }

            foreach (var rentingRequestSerialNumber in rentingRequestSerialNumbers)
            {
                var isValid = await MachineSerialNumberDao.Instance.IsMachineSerialNumberValidToRent(
                    rentingRequestSerialNumber.SerialNumber,
                    rentingRequestSerialNumber.DateStart,
                    rentingRequestSerialNumber.DateEnd);

                if (!isValid)
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

            IList<MachineSerialNumberComponent> machineSerialNumberComponents = new List<MachineSerialNumberComponent>();

            foreach (var componentMachine in componentMachineList)
            {
                var productComponentStatus = new MachineSerialNumberComponent
                {
                    SerialNumber = createSerialMachineNumberDto.SerialNumber,
                    MachineComponentId = componentMachine.MachineComponentId,
                    Quantity = componentMachine.Quantity,
                    Status = MachineSerialNumberComponentStatusEnum.Normal.ToString()
                };

                machineSerialNumberComponents.Add(productComponentStatus);
            }

            if (machineSerialNumberComponents.Count == 0)
            {
                serialMachine.MachineSerialNumberComponents = null;
            }
            else
            {
                serialMachine.MachineSerialNumberComponents = machineSerialNumberComponents;
            }

            MachineSerialNumberLog log = new MachineSerialNumberLog
            {
                SerialNumber = serialMachine.SerialNumber,
                AccountTriggerId = accountId,
                Action = "Tạo mới một máy serial",
                Type = MachineSerialNumberLogTypeEnum.Machine.ToString(),
                DateCreate = now
            };

            serialMachine.MachineSerialNumberLogs = [log];

            await MachineSerialNumberDao.Instance.CreateAsync(serialMachine);

        }

        public async Task Delete(string serialNumber)
        {
            await MachineSerialNumberDao.Instance.Delete(serialNumber);
        }

        public async Task<IEnumerable<MachineSerialNumberComponentDto>> GetMachineComponent(string serialNumber)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumberDetail(serialNumber);

            if (machineSerialNumber.MachineSerialNumberComponents.IsNullOrEmpty())
            {
                return new List<MachineSerialNumberComponentDto>();
            }

            return _mapper.Map<IEnumerable<MachineSerialNumberComponentDto>>(machineSerialNumber.MachineSerialNumberComponents);
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

        public async Task UpdateMachineSerialNumber(string serialNumber, MachineSerialNumberUpdateDto machineSerialNumberUpdateDto, int accountId)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber != null)
            {
                var oldStatus = machineSerialNumber.Status;

                machineSerialNumber.ActualRentPrice = machineSerialNumberUpdateDto.ActualRentPrice;
                machineSerialNumber.RentDaysCounter = machineSerialNumberUpdateDto.RentDaysCounter;
                machineSerialNumber.Status = machineSerialNumberUpdateDto.Status;

                var machineSerialNumberLog = new MachineSerialNumberLog
                {
                    SerialNumber = serialNumber,
                    AccountTriggerId = accountId,
                    DateCreate = DateTime.Now,
                    Type = MachineSerialNumberLogTypeEnum.Machine.ToString(),
                    Action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(oldStatus)}] thành [{EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(machineSerialNumberUpdateDto.Status)}]",
                };

                await MachineSerialNumberLogDao.Instance.CreateAsync(machineSerialNumberLog);

                await MachineSerialNumberDao.Instance.UpdateAsync(machineSerialNumber);
            }
        }

        private async Task UpdateStatusInternal(string serialNumber, string status, int accountId, string? note)
        {
            var serialMachine = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(serialNumber);

            if (serialMachine == null)
            {
                throw new Exception(MessageConstant.MachineSerialNumber.MachineSerialNumberNotFound);
            }

            if (serialMachine.Status == status)
            {
                return;
            }

            var oldStatus = serialMachine.Status;
            serialMachine.Status = status;

            var action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(oldStatus)}] thành [{EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(status)}]";

            if (!note.IsNullOrEmpty())
            {
                action += $". Đi kèm ghi chú: {note}";
            }

            var log = new MachineSerialNumberLog
            {
                SerialNumber = serialNumber,
                AccountTriggerId = accountId,
                DateCreate = DateTime.Now,
                Type = MachineSerialNumberLogTypeEnum.Machine.ToString(),
                Action = action,
            };

            await MachineSerialNumberDao.Instance.UpdateAsync(serialMachine);

            await MachineSerialNumberLogDao.Instance.CreateAsync(log);
        }

        public async Task UpdateStatus(string serialNumber, string status, int accountId)
        {
            await this.UpdateStatusInternal(serialNumber, status, accountId, null);
        }

        public async Task UpdateStatus(string serialNumber, string status, int staffId, string note)
        {
            await this.UpdateStatusInternal(serialNumber, status, staffId, note);
        }
    }
}
