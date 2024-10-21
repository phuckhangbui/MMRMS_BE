using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.DeliveryTask;
using Repository.Interface;

namespace Repository.Implement
{
    public class DeliveryTaskRepository : IDeliveryTaskRepository
    {
        private readonly IMapper _mapper;

        public DeliveryTaskRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task AssignDeliveryTaskToStaff(int managerId, AssignDeliveryTaskDto assignDeliveryTaskDto)
        {
            var DeliveryTask = await DeliveryTaskDao.Instance.GetDeliveryTask(assignDeliveryTaskDto.DeliveryTaskId);

            DeliveryTask.Status = DeliveryTasktatusEnum.Assigned.ToString();

            DeliveryTask.StaffId = assignDeliveryTaskDto.StaffId;
            DeliveryTask.DateShip = assignDeliveryTaskDto.DateShip;

            await DeliveryTaskDao.Instance.UpdateAsync(DeliveryTask);

            var account = await AccountDao.Instance.GetAccountAsyncById(assignDeliveryTaskDto.StaffId);

            string action = $"Assign DeliveryTask task to staff name {account.Name}";

            var DeliveryTaskLog = new DeliveryTaskLog
            {
                DeliveryTaskId = DeliveryTask.DeliveryTaskId,
                AccountTriggerId = managerId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await DeliveryTaskLogDao.Instance.CreateAsync(DeliveryTaskLog);
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveries()
        {
            var list = await DeliveryTaskDao.Instance.GetDeliveries();

            return _mapper.Map<IEnumerable<DeliveryTaskDto>>(list);
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveriesForStaff(int staffId)
        {
            var list = await DeliveryTaskDao.Instance.GetDeliveriesForStaff(staffId);

            return _mapper.Map<IEnumerable<DeliveryTaskDto>>(list);
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveriesOfStaffInADay(int staffId, DateTime dateShip)
        {
            var list = await DeliveryTaskDao.Instance.GetDeliveriesForStaff(staffId);

            var filteredList = list.Where(d => d.DateShip.HasValue && d.DateShip.Value.Date == dateShip.Date).ToList();

            return _mapper.Map<IEnumerable<DeliveryTaskDto>>(filteredList);
        }

        public async Task<DeliveryTaskDto> GetDeliveryTask(int DeliveryTaskId)
        {
            var DeliveryTask = await DeliveryTaskDao.Instance.GetDeliveryTask(DeliveryTaskId);

            return _mapper.Map<DeliveryTaskDto>(DeliveryTask);
        }

        public async Task UpdateDeliveryTaskStatus(int DeliveryTaskId, string status, int accountId)
        {
            var DeliveryTask = await DeliveryTaskDao.Instance.GetDeliveryTask(DeliveryTaskId);

            string oldStatus = DeliveryTask.Status;

            DeliveryTask.Status = status;

            await DeliveryTaskDao.Instance.UpdateAsync(DeliveryTask);

            string action = $"Change status from {oldStatus} to {status}";

            var DeliveryTaskLog = new DeliveryTaskLog
            {
                DeliveryTaskId = DeliveryTaskId,
                AccountTriggerId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await DeliveryTaskLogDao.Instance.CreateAsync(DeliveryTaskLog);
        }
    }
}
