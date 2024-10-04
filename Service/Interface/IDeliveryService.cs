using DTOs.Delivery;

namespace Service.Interface
{
    public interface IDeliveryService
    {
        Task AssignDelivery(int managerId, AssignDeliveryDto assignDeliveryDto);
        Task<IEnumerable<DeliveryDto>> GetDeliveries();
        Task<IEnumerable<DeliveryDto>> GetDeliveries(int staffId);
        Task UpdateDeliveryStatus(int deliveryId, string status, int accountId);
    }
}
