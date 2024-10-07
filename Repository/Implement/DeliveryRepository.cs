using AutoMapper;
using BusinessObject;
using Common.Enum;
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

        public async Task AssignDeliveryToStaff(int managerId, AssignDeliveryDto assignDeliveryDto)
        {
            var delivery = await DeliveryDao.Instance.GetDelivery(assignDeliveryDto.DeliveryId);

            delivery.Status = DeliveryStatusEnum.Assigned.ToString();

            delivery.StaffId = assignDeliveryDto.StaffId;
            delivery.DateShip = assignDeliveryDto.DateShip;

            await DeliveryDao.Instance.UpdateAsync(delivery);

            var account = await AccountDao.Instance.GetAccountAsyncById(assignDeliveryDto.StaffId);

            string action = $"Assign delivery task to staff name {account.Name}";

            var deliveryLog = new DeliveryLog
            {
                DeliveryId = delivery.DeliveryId,
                AccountTriggerId = managerId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await DeliveryLogDao.Instance.CreateAsync(deliveryLog);
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

        public async Task<IEnumerable<DeliveryDto>> GetDeliveriesOfStaffInADay(int staffId, DateTime dateShip)
        {
            var list = await DeliveryDao.Instance.GetDeliveriesForStaff(staffId);

            var filteredList = list.Where(d => d.DateShip.HasValue && d.DateShip.Value.Date == dateShip.Date);

            return _mapper.Map<IEnumerable<DeliveryDto>>(filteredList);
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
                AccountTriggerId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await DeliveryLogDao.Instance.CreateAsync(deliveryLog);
        }
    }
}
