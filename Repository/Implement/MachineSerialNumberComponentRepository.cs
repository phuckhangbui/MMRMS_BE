using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.MachineSerialNumber;
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

        public async Task UpdateComponentStatus(int machineSerialNumberComponentId, string status, int accountId)
        {
            var serialComponent = await MachineSerialNumberComponentDao.Instance.GetComponent(machineSerialNumberComponentId);

            if (serialComponent == null)
            {
                throw new Exception(MessageConstant.MachineSerialNumber.ComponentIdNotFound);
            }

            var now = DateTime.Now;

            serialComponent.Status = status;
            serialComponent.DateModified = now;

            await MachineSerialNumberComponentDao.Instance.UpdateAsync(serialComponent);

            var log = new MachineSerialNumberLog
            {
                SerialNumber = serialComponent.SerialNumber,
                MachineSerialNumberComponentId = serialComponent.MachineSerialNumberComponentId,
                AccountTriggerId = accountId,
                DateCreate = now,
                Type = MachineSerialNumberLogTypeEnum.MachineComponent.ToString(),
                Action = $"Thay đổi trạng thái của bộ phận {serialComponent?.Component?.Component?.ComponentName ?? string.Empty} thành [{EnumExtensions.TranslateStatus<MachineSerialNumberComponentStatusEnum>(status)}]",
            };

            await MachineSerialNumberLogDao.Instance.CreateAsync(log);
        }
    }
}
