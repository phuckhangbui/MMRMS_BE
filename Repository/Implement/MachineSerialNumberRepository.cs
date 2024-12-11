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

        public async Task<bool> CheckMachineSerialNumberValidToRent(List<RentingRequestSerialNumberDto> rentingRequestSerialNumbers)
        {
            if (rentingRequestSerialNumbers.IsNullOrEmpty())
            {
                return false;
            }

            foreach (var rentingRequestSerialNumber in rentingRequestSerialNumbers)
            {
                var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(rentingRequestSerialNumber.SerialNumber);
                if (machineSerialNumber == null ||
                            !(machineSerialNumber.Status.Equals(MachineSerialNumberStatusEnum.Available.ToString()) ||
                              (machineSerialNumber.Status.Equals(MachineSerialNumberStatusEnum.Maintained.ToString()) &&
                               machineSerialNumber.ExpectedAvailableDate != null &&
                               rentingRequestSerialNumber.DateStart >= machineSerialNumber.ExpectedAvailableDate)))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task CreateMachineSerialNumber(MachineSerialNumberDto serialMachineNumberDto, IEnumerable<MachineComponentDto> componentMachineList, int accountId)
        {
            var now = DateTime.Now;


            IList<MachineSerialNumberComponent> machineSerialNumberComponents = new List<MachineSerialNumberComponent>();

            foreach (var componentMachine in componentMachineList)
            {
                var productComponentStatus = new MachineSerialNumberComponent
                {
                    SerialNumber = serialMachineNumberDto.SerialNumber,
                    MachineComponentId = componentMachine.MachineComponentId,
                    Quantity = componentMachine.Quantity,
                    Status = MachineSerialNumberComponentStatusEnum.Normal.ToString()
                };

                machineSerialNumberComponents.Add(productComponentStatus);
            }

            MachineSerialNumber machineSerialNumber = _mapper.Map<MachineSerialNumber>(serialMachineNumberDto);


            if (machineSerialNumberComponents.Count == 0)
            {
                machineSerialNumber.MachineSerialNumberComponents = null;
            }
            else
            {
                machineSerialNumber.MachineSerialNumberComponents = machineSerialNumberComponents;
            }

            MachineSerialNumberLog log = new MachineSerialNumberLog
            {
                SerialNumber = serialMachineNumberDto.SerialNumber,
                AccountTriggerId = accountId,
                Action = "Tạo mới một máy serial",
                Type = MachineSerialNumberLogTypeEnum.Machine.ToString(),
                DateCreate = now
            };

            machineSerialNumber.MachineSerialNumberLogs = [log];

            await MachineSerialNumberDao.Instance.CreateAsync(machineSerialNumber);

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

        public async Task UpdateRentDaysCounterMachineSerialNumber(MachineSerialNumberDto machineSerialNumberDto, int accountId)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(machineSerialNumberDto.SerialNumber);

            if (machineSerialNumber != null)
            {
                var oldRentPrice = machineSerialNumber.ActualRentPrice;

                machineSerialNumber.ActualRentPrice = machineSerialNumberDto.ActualRentPrice;
                machineSerialNumber.MachineConditionPercent = machineSerialNumberDto.MachineConditionPercent;
                machineSerialNumber.RentDaysCounter = machineSerialNumberDto.RentDaysCounter;

                if (oldRentPrice != machineSerialNumberDto.ActualRentPrice)
                {
                    var machineSerialNumberLog = new MachineSerialNumberLog
                    {
                        SerialNumber = machineSerialNumberDto.SerialNumber,
                        AccountTriggerId = accountId,
                        DateCreate = DateTime.Now,
                        Type = MachineSerialNumberLogTypeEnum.Machine.ToString()
                    };

                    machineSerialNumber.ActualRentPrice = machineSerialNumberDto.ActualRentPrice;
                    string action = $"Thay đổi tiền thuê theo ngày của máy từ [{NumberExtension.FormatToVND((double)oldRentPrice)}] thành [{NumberExtension.FormatToVND((double)machineSerialNumberDto.ActualRentPrice)}]";
                    machineSerialNumberLog.Action = action;

                    await MachineSerialNumberLogDao.Instance.CreateAsync(machineSerialNumberLog);
                }

                await MachineSerialNumberDao.Instance.UpdateAsync(machineSerialNumber);
            }
        }

        public async Task UpdateMachineSerialNumber(string serialNumber, MachineSerialNumberUpdateDto machineSerialNumberUpdateDto, int accountId)
        {
            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(serialNumber);

            if (machineSerialNumber != null)
            {
                var oldStatus = machineSerialNumber.Status;
                var oldRentPrice = machineSerialNumber.ActualRentPrice;

                machineSerialNumber.ActualRentPrice = machineSerialNumberUpdateDto.ActualRentPrice;
                var machineSerialNumberLog = new MachineSerialNumberLog
                {
                    SerialNumber = serialNumber,
                    AccountTriggerId = accountId,
                    DateCreate = DateTime.Now,
                    Type = MachineSerialNumberLogTypeEnum.Machine.ToString()
                };

                if (oldStatus != machineSerialNumberUpdateDto.Status)
                {
                    machineSerialNumber.Status = machineSerialNumberUpdateDto.Status;
                    string action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(oldStatus)}] thành [{EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(machineSerialNumberUpdateDto.Status)}]";
                    machineSerialNumberLog.Action = action;
                }
                else if (oldRentPrice != machineSerialNumberUpdateDto.ActualRentPrice)
                {
                    machineSerialNumber.ActualRentPrice = machineSerialNumberUpdateDto.ActualRentPrice;
                    string action = $"Thay đổi tiền thuê theo ngày của máy từ [{NumberExtension.FormatToVND((double)oldRentPrice)}] thành [{NumberExtension.FormatToVND((double)machineSerialNumberUpdateDto.ActualRentPrice)}]";
                    machineSerialNumberLog.Action = action;
                }

                await MachineSerialNumberLogDao.Instance.CreateAsync(machineSerialNumberLog);
            }


            await MachineSerialNumberDao.Instance.UpdateAsync(machineSerialNumber);
        }

        private async Task UpdateStatusInternal(string serialNumber, string status, int accountId, string? note, DateTime? expectedAvailableDate)
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

            if (expectedAvailableDate == null)
            {
                serialMachine.ExpectedAvailableDate = expectedAvailableDate;
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
            await this.UpdateStatusInternal(serialNumber, status, accountId, null, null);
        }

        public async Task UpdateStatus(string serialNumber, string status, int staffId, string note)
        {
            await this.UpdateStatusInternal(serialNumber, status, staffId, note, null);
        }

        public async Task UpdateStatus(string serialNumber, string status, int accountId, string? note, DateTime? expectedAvailableDate)
        {
            await UpdateStatusInternal(serialNumber, status, accountId, null, expectedAvailableDate);
        }
    }
}
