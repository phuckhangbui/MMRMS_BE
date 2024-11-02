
namespace Repository.Interface
{
    public interface IMachineSerialNumberLogRepository
    {
        Task WriteMachineLog(string serialNumber, string action, int accountTriggerId);

        Task WriteComponentLog(string serialNumber, int machineSerialNumberComponentId, string action, int accountTriggerId);
    }
}
