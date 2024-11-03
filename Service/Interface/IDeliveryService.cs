using DTOs.Delivery;

namespace Service.Interface
{
    public interface IDeliverService
    {
        Task CreateDeliveryTask(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries();
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries(int staffId);
        Task<DeliveryTaskDetailDto> GetDeliveryDetail(int deliveryTaskId);
        Task StaffCompleteDelivery(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto, int accountId);
        Task UpdateDeliveryStatusToDelivering(int deliveryTaskId, int accountId);
        Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId);
    }
}
