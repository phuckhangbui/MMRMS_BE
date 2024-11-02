using BusinessObject;
using Common.Enum;
using DAO;
using Repository.Interface;

namespace Service
{
    public class MachineSerialNumberLogRepository : IMachineSerialNumberLogRepository
    {
        public async Task WriteComponentLog(string serialNumber, int machineSerialNumberComponentId, string action, int accountTriggerId)
        {
            var log = new MachineSerialNumberLog
            {
                SerialNumber = serialNumber,
                MachineSerialNumberComponentId = machineSerialNumberComponentId,
                Action = action,
                AccountTriggerId = accountTriggerId,
                DateCreate = DateTime.Now,
                Type = MachineSerialNumberLogTypeEnum.MachineComponent.ToString(),
            };

            await MachineSerialNumberLogDao.Instance.CreateAsync(log);
        }


        public async Task WriteMachineLog(string serialNumber, string action, int accountTriggerId)
        {
            var log = new MachineSerialNumberLog
            {
                SerialNumber = serialNumber,
                Action = action,
                AccountTriggerId = accountTriggerId,
                DateCreate = DateTime.Now,
                Type = MachineSerialNumberLogTypeEnum.Machine.ToString(),
            };

            await MachineSerialNumberLogDao.Instance.CreateAsync(log);
        }
    }
}
