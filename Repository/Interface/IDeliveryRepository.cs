using DTOs.Delivery;

namespace Repository.Interface
{
    public interface IDeliveryRepository
    {
        Task<IEnumerable<DeliveryDto>> GetDeliveries();
        Task<IEnumerable<DeliveryDto>> GetDeliveriesForStaff(int staffId);
        Task<DeliveryDto> GetDelivery(int deliveryId);
        Task UpdateDeliveryStatus(int deliveryId, string status);
    }
}
