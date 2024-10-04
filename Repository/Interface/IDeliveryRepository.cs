using DTOs.Delivery;

namespace Repository.Interface
{
    public interface IDeliveryRepository
    {
        Task AssignDeliveryToStaff(int managerId, AssignDeliveryDto assignDeliveryDto);
        Task<IEnumerable<DeliveryDto>> GetDeliveries();
        Task<IEnumerable<DeliveryDto>> GetDeliveriesForStaff(int staffId);
        Task<IEnumerable<DeliveryDto>> GetDeliveriesOfStaffInADay(int staffId, DateTime dateShip);
        Task<DeliveryDto> GetDelivery(int deliveryId);
        Task UpdateDeliveryStatus(int deliveryId, string status, int accountId);
    }
}
