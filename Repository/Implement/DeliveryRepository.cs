using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.Delivery;
using Repository.Interface;

namespace Repository.Implement
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly IMapper _mapper;

        public DeliveryRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeliveryDto>> GetDeliveries()
        {
            var list = await DeliveryDao.Instance.GetDeliveries();

            return _mapper.Map<IEnumerable<DeliveryDto>>(list);
        }

        public async Task<IEnumerable<DeliveryDto>> GetDeliveriesForStaff(int staffId)
        {
            var list = await DeliveryDao.Instance.GetDeliveriesForStaff(staffId);

            return _mapper.Map<IEnumerable<DeliveryDto>>(list);
        }

        public async Task<DeliveryDto> GetDelivery(int deliveryId)
        {
            var delivery = await DeliveryDao.Instance.GetDelivery(deliveryId);

            return _mapper.Map<DeliveryDto>(delivery);
        }

        public async Task UpdateDeliveryStatus(int deliveryId, string status, int accountId)
        {
            var delivery = await DeliveryDao.Instance.GetDelivery(deliveryId);

            string oldStatus = delivery.Status;

            delivery.Status = status;

            await DeliveryDao.Instance.UpdateAsync(delivery);

            string action = $"Change status from {oldStatus} to {status}";

            var deliveryLog = new DeliveryLog
            {
                DeliveryId = deliveryId,
                AccountId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await DeliveryLogDao.Instance.CreateAsync(deliveryLog);
        }
    }
}
