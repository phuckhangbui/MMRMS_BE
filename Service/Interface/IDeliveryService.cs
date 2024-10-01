using DTOs.Delivery;

namespace Service.Interface
{
    public interface IDeliveryService
    {
        Task<IEnumerable<DeliveryDto>> GetDeliveries();
        Task<IEnumerable<DeliveryDto>> GetDeliveries(int staffId);
        Task UpdateDeliveryStatus(int deliveryId, string status);
    }
}
