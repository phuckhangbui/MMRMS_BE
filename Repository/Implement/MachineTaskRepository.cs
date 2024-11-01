using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.ComponentReplacementTicket;
using DTOs.MachineTask;
using Repository.Interface;

namespace Repository.Implement
{
    public class MachineTaskRepository : IMachineTaskRepository
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMachineCheckRequestRepository _machineCheckRequestRepository;
        private readonly IMapper _mapper;

        public MachineTaskRepository(IMapper mapper, IAccountRepository accountRepository, IMachineCheckRequestRepository MachineCheckRequestRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _machineCheckRequestRepository = MachineCheckRequestRepository;
        }

        public async Task<MachineTaskDto> CreateMachineTaskWithRequest(int managerId, CreateMachineTaskCheckMachineDto createMachineTaskDto)
        {
            var staffAccount = await _accountRepository.GetAccounById(createMachineTaskDto.StaffId);

            var request = await MachineCheckRequestDao.Instance.GetMachineCheckRequest(createMachineTaskDto.RequestId);

            var now = DateTime.Now;

            var task = new MachineTask
            {
                TaskTitle = createMachineTaskDto.TaskTitle,
                Content = createMachineTaskDto.TaskContent,
                StaffId = createMachineTaskDto.StaffId,
                ManagerId = managerId,
                Type = MachineTaskTypeEnum.MachineryCheck.ToString(),
                DateCreate = now,
                DateStart = createMachineTaskDto.DateStart,
                Status = MachineTaskStatusEnum.Created.ToString(),
                Note = createMachineTaskDto.Note,
                MachineCheckRequestId = createMachineTaskDto.RequestId,
                ContractId = request.ContractId
            };

            var taskLog = new MachineTaskLog
            {
                Action = $"Tạo và giao công việc kiểm tra máy cho {staffAccount.Name}",
                DateCreate = now,
                AccountTriggerId = managerId,
            };

            task = await MachineTaskDao.Instance.CreateMachineTaskBaseOnRequest(task, taskLog);

            return _mapper.Map<MachineTaskDto>(task);
        }

        public async Task Delete(int taskId)
        {
            var MachineTask = await MachineTaskDao.Instance.GetMachineTask(taskId);

            await MachineTaskDao.Instance.RemoveAsync(MachineTask);
        }

        public async Task<MachineTaskDto> GetMachineTask(int taskId)
        {
            var MachineTask = await MachineTaskDao.Instance.GetMachineTask(taskId);

            return _mapper.Map<MachineTaskDto>(MachineTask);
        }

        public async Task<IEnumerable<MachineTaskDto>> GetMachineTaskByStaff(int staffId)
        {
            var list = await MachineTaskDao.Instance.GetMachineTasks();

            var resultList = list.Where(t => t.StaffId == staffId).ToList();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(resultList);
        }

        public async Task<MachineTaskDisplayDetail> GetMachineTaskDetail(int taskId)
        {
            var machineTask = await MachineTaskDao.Instance.GetMachineTaskDetail(taskId);

            List<MachineTaskLog> machineTaskLogs = (List<MachineTaskLog>)machineTask.MachineTaskLogs;

            var taskLogsDto = _mapper.Map<List<MachineTaskLogDto>>(machineTaskLogs);

            var componentReplacementTicketCreateFromTaskList = _mapper.Map<List<ComponentReplacementTicketDto>>(machineTask.ComponentReplacementTicketsCreateFromTask);

            var taskDetail = _mapper.Map<MachineTaskDisplayDetail>(machineTask);
            taskDetail.TaskLogs = taskLogsDto;
            taskDetail.ComponentReplacementTicketCreateFromTaskList = componentReplacementTicketCreateFromTaskList;

            return taskDetail;
        }

        public async Task<IEnumerable<MachineTaskDto>> GetMachineTasks()
        {
            var list = await MachineTaskDao.Instance.GetMachineTasks();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(list);
        }

        public async Task<IEnumerable<MachineTaskDto>> GetMachineTasksForStaff(int staffId, DateOnly dateStart, DateOnly dateEnd)
        {
            var list = await MachineTaskDao.Instance.GetMachineTaskStaff(staffId, dateStart, dateEnd);

            if (list == null)
            {
                return new List<MachineTaskDto>();
            }

            list = list.Where(t => t.Status != MachineTaskStatusEnum.Canceled.ToString()).ToList();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(list);
        }

        public async Task<IEnumerable<MachineTaskDto>> GetMachineTasksInADate(DateOnly date)
        {
            var list = await MachineTaskDao.Instance.GetMachineTasksInADate(date);

            if (list == null)
            {
                return new List<MachineTaskDto>();
            }

            list = list.Where(t => t.Status != MachineTaskStatusEnum.Canceled.ToString()).ToList();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(list);
        }

        public async Task<IEnumerable<MachineTaskDto>> GetTaskOfStaffInADay(int staffId, DateTime date)
        {
            var list = await MachineTaskDao.Instance.GetMachineTasks();

            var staffList = list.Where(t => t.StaffId == staffId).ToList();

            var filteredList = staffList.Where(d => d.DateStart.HasValue && d.DateStart.Value.Date == date.Date && d.Status != MachineTaskStatusEnum.Canceled.ToString()).ToList();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(filteredList);
        }

        public async Task UpdateTaskStatus(int machineTaskId, string status, int accountId, string? confirmationPictureUrl)
        {
            var machineTask = await MachineTaskDao.Instance.GetMachineTask(machineTaskId);

            string oldStatus = machineTask.Status;
            if (confirmationPictureUrl != null && status == MachineTaskStatusEnum.Completed.ToString())
            {
                machineTask.ConfirmationPictureUrl = confirmationPictureUrl;
            }

            machineTask.Status = status;

            if (status == MachineTaskStatusEnum.Completed.ToString())
            {
                machineTask.DateCompleted = DateTime.Now;
            }

            await MachineTaskDao.Instance.UpdateAsync(machineTask);

            string action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<MachineTaskStatusEnum>(oldStatus)}] trở thành [{EnumExtensions.TranslateStatus<MachineTaskStatusEnum>(status)}]";

            var taskLog = new MachineTaskLog
            {
                MachineTaskId = machineTaskId,
                AccountTriggerId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await MachineTaskLogDao.Instance.CreateAsync(taskLog);


        }

    }
}
