using DTOs.Delivery;

namespace Service.Interface
{
    public interface IDeliverService
    {
        Task CreateDeliveryTask(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries();
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveries(int staffId);
        Task<IEnumerable<DeliveryTaskDto>> GetDeliveriesForCustomer(int customerId);
        Task<DeliveryTaskDetailDto> GetDeliveryDetail(int deliveryTaskId);
        Task StaffCompleteDelivery(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto, int accountId);
        Task StaffFailDelivery(StaffFailDeliveryTaskDto staffFailDeliveryTask, int accountId);
        Task UpdateContractDeliveryStatusToProcessedAfterFailure(int contractDeliveryId, int accountId);
        Task UpdateDeliveryStatusToDelivering(int deliveryTaskId, int accountId);
        Task UpdateDeliveryStatusToProcessedAfterFailure(int deliveryTaskId, int accountId);
    }
}
