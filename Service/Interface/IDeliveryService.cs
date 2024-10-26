using DTOs.DeliveryTask;

namespace Service.Interface
{
    public interface IDeliverService
    {
        Task CreateDeliveryTask(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries();
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries(int staffId);
        Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId);
    }
}
