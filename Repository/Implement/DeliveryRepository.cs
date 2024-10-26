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

        public async Task<DeliveryTaskDto> CreateDeliveryTaskToStaff(int managerId, CreateDeliveryTaskDto createDeliveryTaskDto)
        {
            var now = DateTime.Now;

            var deliveryTask = new DeliveryTask
            {
                StaffId = createDeliveryTaskDto.StaffId,
                DateShip = createDeliveryTaskDto.DateShip,
                DateCreate = now,
                Status = DeliveryTaskStatusEnum.Created.ToString(),
                Type = DeliveryTaskTypeEnum.Delivery.ToString()
            };

            var account = await AccountDao.Instance.GetAccountAsyncById(createDeliveryTaskDto.StaffId);

            string action = $"Tạo và giao đơn giao cho nhân viên tên: {account.Name}";

            var deliveryTaskLog = new DeliveryTaskLog
            {
                DeliveryTaskId = deliveryTask.DeliveryTaskId,
                AccountTriggerId = managerId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            var listContractDelivery = new List<ContractDelivery>();
            foreach (string contractId in createDeliveryTaskDto.ContractIdList)
            {
                var contractDelivery = new ContractDelivery
                {
                    ContractId = contractId,
                    DeliveryTaskId = deliveryTask.DeliveryTaskId,
                    Status = ContractDeliveryStatusEnum.Pending.ToString(),
                };

                listContractDelivery.Add(contractDelivery);
            }

            deliveryTask = await DeliveryTaskDao.Instance.CreateDelivery(deliveryTask, listContractDelivery, deliveryTaskLog);

            var deliveryTaskDto = _mapper.Map<DeliveryTaskDto>(deliveryTask);

            return deliveryTaskDto;
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
