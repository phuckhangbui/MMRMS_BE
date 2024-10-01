using DTOs.Delivery;
using Service.Interface;

namespace Service.Implement
{
    public class DeliveryService : IDeliveryService
    {
        public Task<IEnumerable<DeliveryDto>> GetDeliveries()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeliveryDto>> GetDeliveries(int staffId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDeliveryStatus(int deliveryId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
