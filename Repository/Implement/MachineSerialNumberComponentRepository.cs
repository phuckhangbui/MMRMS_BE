using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.MachineSerialNumber;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class MachineSerialNumberComponentRepository : IMachineSerialNumberComponentRepository
    {
        private readonly IMapper _mapper;

        public MachineSerialNumberComponentRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<MachineSerialNumberComponentDto> GetComponent(int machineSerialNumberComponentId)
        {
            var serialComponent = await MachineSerialNumberComponentDao.Instance.GetComponent(machineSerialNumberComponentId);

            return _mapper.Map<MachineSerialNumberComponentDto>(serialComponent);
        }

        private async Task UpdateComponentStatusInternal(int machineSerialNumberComponentId, string status, int accountId, string note)
        {
            var serialComponent = await MachineSerialNumberComponentDao.Instance.GetComponent(machineSerialNumberComponentId);

            if (serialComponent == null)
            {
                throw new Exception(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
            }

            var oldStatus = serialComponent.Status;

            if (oldStatus == status)
            {
                return;
            }

            var now = DateTime.Now;

            serialComponent.Status = status;
            serialComponent.DateModified = now;

            await MachineSerialNumberComponentDao.Instance.UpdateAsync(serialComponent);
            var action = $"Thay đổi trạng thái của bộ phận {serialComponent?.Component?.Component?.ComponentName ?? string.Empty} từ [{EnumExtensions.TranslateStatus<MachineSerialNumberComponentStatusEnum>(oldStatus)}] thành [{EnumExtensions.TranslateStatus<MachineSerialNumberComponentStatusEnum>(status)}]";

            if (!note.IsNullOrEmpty())
            {
                action += $". Đi kèm ghi chú: {note}";
            }

            var log = new MachineSerialNumberLog
            {
                SerialNumber = serialComponent.SerialNumber,
                MachineSerialNumberComponentId = serialComponent.MachineSerialNumberComponentId,
                AccountTriggerId = accountId,
                DateCreate = now,
                Type = MachineSerialNumberLogTypeEnum.MachineComponent.ToString(),
                Action = action
            };

            await MachineSerialNumberLogDao.Instance.CreateAsync(log);
        }

        public async Task UpdateComponentStatus(int machineSerialNumberComponentId, string status, int accountId)
        {
            await this.UpdateComponentStatusInternal(machineSerialNumberComponentId, status, accountId, null);
        }

        public async Task UpdateComponentStatus(int machineSerialNumberComponentId, string status, int staffId, string note)
        {
            await this.UpdateComponentStatusInternal(machineSerialNumberComponentId, status, staffId, note);
        }
    }
}
