using DTOs.Delivery;
using DTOs.DeliveryTask;

namespace Service.Interface
{
    public interface IDeliverService
    {
        Task CreateDeliveryTask(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries();
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries(int staffId);
        Task<DeliveryTaskDetailDto> GetDeliveryDetail(int deliveryTaskId);
        Task UpdateDeliveryStatusToDelivering(int deliveryTaskId, int accountId);
        Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId);
    }
}
