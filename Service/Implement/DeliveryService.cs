using Common;
using DAO.Enum;
using DTOs.Delivery;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<IEnumerable<DeliveryDto>> GetDeliveries()
        {
            return await _deliveryRepository.GetDeliveries();
        }

        public async Task<IEnumerable<DeliveryDto>> GetDeliveries(int staffId)
        {
            return await _deliveryRepository.GetDeliveriesForStaff(staffId);
        }

        public async Task UpdateDeliveryStatus(int deliveryId, string status, int accountId)
        {
            DeliveryDto deliveryDto = await _deliveryRepository.GetDelivery(deliveryId);

            if (deliveryDto == null)
            {
                throw new ServiceException(MessageConstant.Delivery.DeliveryNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(DeliveryStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.Delivery.StatusNotAvailable);
            }

            //business logic here, fix later

            await _deliveryRepository.UpdateDeliveryStatus(deliveryId, status, accountId);
        }
    }
}
