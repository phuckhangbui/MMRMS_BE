using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.Delivery;
using Repository.Interface;
using System.Globalization;

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

            if (!DateTime.TryParseExact(createDeliveryTaskDto.DateShip, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new Exception("Format ngày không đúng, xin hãy dùng 'yyyy-MM-dd'.");
            }

            var deliveryTask = new DeliveryTask
            {
                ManagerId = managerId,
                StaffId = createDeliveryTaskDto.StaffId,
                DateShip = parsedDate,
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

        public async Task CompleteFullyAllDeliveryTask(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto)
        {
            var now = DateTime.Now;

            var delivery = await DeliveryTaskDao.Instance.GetDeliveryDetail(staffUpdateDeliveryTaskDto.DeliveryTaskId);

            delivery.Status = DeliveryTaskStatusEnum.Completed.ToString();
            delivery.DateCompleted = now;
            delivery.ReceiverName = staffUpdateDeliveryTaskDto.ReceiverName;
            delivery.Note = staffUpdateDeliveryTaskDto.Note;
            delivery.ConfirmationPictureUrl = staffUpdateDeliveryTaskDto.ConfirmationPictureUrl;

            foreach (var (old, update) in delivery.ContractDeliveries
                .OrderBy(d => d.ContractDeliveryId)
                .Zip(staffUpdateDeliveryTaskDto.ContractDeliveries.OrderBy(d => d.ContractDeliveryId), (delivery, update) => (delivery, update)))
            {
                old.Note = update.Note;
                old.PictureUrl = update.PictureUrl;
                old.Status = ContractDeliveryStatusEnum.Success.ToString();
            }

            string action = "Đơn giao hàng được hoàn thành toàn bộ";

            var newLogs = new DeliveryTaskLog
            {
                DeliveryTaskId = delivery.DeliveryTaskId,
                DateCreate = now,
                AccountTriggerId = delivery.StaffId,
                Action = action,
            };


            await DeliveryTaskDao.Instance.UpdateDeliveryAndContractDelivery(delivery, newLogs);

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

        public async Task<DeliveryTaskDetailDto> GetDeliveryTaskDetail(int deliveryTaskId)
        {
            var delivery = await DeliveryTaskDao.Instance.GetDeliveryDetail(deliveryTaskId);

            if (delivery == null)
            {
                throw new Exception(MessageConstant.DeliveryTask.DeliveryTaskNotFound);
            }

            var deliveryTaskDto = _mapper.Map<DeliveryTaskDto>(delivery);

            var deliveryLogList = _mapper.Map<IEnumerable<DeliveryTaskLogDto>>(delivery.DeliveryTaskLogs);

            var contractDeliveryList = _mapper.Map<IEnumerable<ContractDeliveryDto>>(delivery.ContractDeliveries);

            return new DeliveryTaskDetailDto
            {
                DeliveryTask = deliveryTaskDto,
                DeliveryTaskLogs = deliveryLogList,
                ContractDeliveries = contractDeliveryList
            };
        }

        public async Task UpdateDeliveryTaskStatus(int deliveryTaskId, string status, int accountId)
        {
            var deliveryTask = await DeliveryTaskDao.Instance.GetDeliveryTask(deliveryTaskId);

            string oldStatus = deliveryTask.Status;

            deliveryTask.Status = status;

            await DeliveryTaskDao.Instance.UpdateAsync(deliveryTask);

            string action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<DeliveryTaskStatusEnum>(oldStatus)}] trở thành [{EnumExtensions.TranslateStatus<DeliveryTaskStatusEnum>(status)}]";

            var DeliveryTaskLog = new DeliveryTaskLog
            {
                DeliveryTaskId = deliveryTaskId,
                AccountTriggerId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await DeliveryTaskLogDao.Instance.CreateAsync(DeliveryTaskLog);
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveryTasksForStaff(int staffId, DateOnly dateStart, DateOnly dateEnd)
        {
            var list = await DeliveryTaskDao.Instance.GetDeliverTaskStaff(staffId, dateStart, dateEnd);

            if (list == null)
            {
                return new List<DeliveryTaskDto>();
            }

            return _mapper.Map<IEnumerable<DeliveryTaskDto>>(list);
        }

        public async Task<IEnumerable<DeliveryTaskDto>> GetDeliveryTasksInADate(DateOnly date)
        {
            var list = await DeliveryTaskDao.Instance.GetDeliveryTasksInADate(date);

            if (list == null)
            {
                return new List<DeliveryTaskDto>();
            }

            return _mapper.Map<IEnumerable<DeliveryTaskDto>>(list);
        }

        public async Task FailDeliveryTask(StaffFailDeliveryTaskDto staffFailDeliveryTask)
        {
            var now = DateTime.Now;

            var delivery = await DeliveryTaskDao.Instance.GetDeliveryDetail(staffFailDeliveryTask.DeliveryTaskId);

            delivery.Status = DeliveryTaskStatusEnum.Fail.ToString();
            delivery.DateCompleted = now;
            delivery.Note = staffFailDeliveryTask.Note;

            foreach (var contractDelivery in delivery.ContractDeliveries)
            {
                contractDelivery.Note = staffFailDeliveryTask.Note;
                contractDelivery.Status = ContractDeliveryStatusEnum.Fail.ToString();
            }

            string action = $"Đơn giao hàng đã thất bại toàn bộ, ghi chú của người giao hàng: [{staffFailDeliveryTask.Note}]";

            var newLogs = new DeliveryTaskLog
            {
                DeliveryTaskId = delivery.DeliveryTaskId,
                DateCreate = now,
                AccountTriggerId = delivery.StaffId,
                Action = action,
            };

            await DeliveryTaskDao.Instance.UpdateDeliveryAndContractDelivery(delivery, newLogs);
        }
    }
}
