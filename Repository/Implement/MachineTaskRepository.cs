using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.ComponentReplacementTicket;
using DTOs.MachineTask;
using Repository.Interface;
using System.Globalization;

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

        private async Task<MachineTaskDto> CreateMachineTaskInternal(int managerId,
                                                             CreateMachineTaskDtoBase createMachineTaskDto,
                                                             string taskType,
                                                             string logAction)
        {
            // Parse DateStart
            if (!DateTime.TryParseExact(createMachineTaskDto.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new Exception("Format ngày không đúng, xin hãy dùng 'yyyy-MM-dd'.");
            }

            var staffAccount = await _accountRepository.GetAccounById(createMachineTaskDto.StaffId);
            var now = DateTime.Now;

            var task = new MachineTask
            {
                TaskTitle = createMachineTaskDto.TaskTitle,
                Content = createMachineTaskDto.TaskContent,
                StaffId = createMachineTaskDto.StaffId,
                ManagerId = managerId,
                Type = taskType,
                DateCreate = now,
                DateStart = parsedDate,
                Status = MachineTaskEnum.Created.ToString(),
                Note = createMachineTaskDto.Note
            };

            if (createMachineTaskDto is CreateMachineTaskCheckRequestDto checkMachineDto)
            {
                var request = await MachineCheckRequestDao.Instance.GetMachineCheckRequest(checkMachineDto.RequestId);
                task.MachineCheckRequestId = checkMachineDto.RequestId;
                task.ContractId = request.ContractId;
            }
            else if (createMachineTaskDto is CreateMachineTaskContractTerminationDto terminationDto)
            {
                task.ContractId = terminationDto.ContractId;
            }

            var taskLog = new MachineTaskLog
            {
                Action = logAction.Replace("{staffName}", staffAccount.Name),
                DateCreate = now,
                AccountTriggerId = managerId
            };

            task = await MachineTaskDao.Instance.CreateMachineTaskBaseOnRequest(task, taskLog);

            return _mapper.Map<MachineTaskDto>(task);
        }
        public async Task<MachineTaskDto> CreateMachineTaskContractTermination(int managerId, CreateMachineTaskContractTerminationDto createMachineTaskDto)
        {
            return await CreateMachineTaskInternal(managerId, createMachineTaskDto,
                                   MachineTaskTypeEnum.ContractTerminationCheck.ToString(),
                                   "Tạo và giao công việc kiêm tra máy chấm dứt hợp đồng cho {staffName}");
        }

        public async Task<MachineTaskDto> CreateMachineTaskWithRequest(int managerId, CreateMachineTaskCheckRequestDto createMachineTaskDto)
        {

            return await CreateMachineTaskInternal(managerId, createMachineTaskDto,
                                        MachineTaskTypeEnum.MachineryCheckRequest.ToString(),
                                        "Tạo và giao công việc kiểm tra máy cho {staffName}");
        }

        public async Task<MachineTaskDto> CreateMachineTaskCheckMachineWhenDeliveryFail(int managerId, CreateMachineTaskContractTerminationDto createMachineTaskDto)
        {
            return await CreateMachineTaskInternal(managerId, createMachineTaskDto,
                                  MachineTaskTypeEnum.DeliveryFailCheckRequest.ToString(),
                                  "Tạo và giao công việc kiêm tra máy khi khách từ chối nhận máy lúc giao cho {staffName}");
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

            list = list.Where(t => t.Status != MachineTaskEnum.Canceled.ToString()).ToList();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(list);
        }

        public async Task<IEnumerable<MachineTaskDto>> GetMachineTasksInADate(DateOnly date)
        {
            var list = await MachineTaskDao.Instance.GetMachineTasksInADate(date);

            if (list == null)
            {
                return new List<MachineTaskDto>();
            }

            list = list.Where(t => t.Status != MachineTaskEnum.Canceled.ToString()).ToList();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(list);
        }

        public async Task<IEnumerable<MachineTaskDto>> GetTaskOfStaffInADay(int staffId, DateTime date)
        {
            var list = await MachineTaskDao.Instance.GetMachineTasks();

            var staffList = list.Where(t => t.StaffId == staffId).ToList();

            var filteredList = staffList.Where(d => d.DateStart.HasValue && d.DateStart.Value.Date == date.Date && d.Status != MachineTaskEnum.Canceled.ToString()).ToList();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(filteredList);
        }

        public async Task UpdateTaskStatus(int machineTaskId, string status, int accountId, string? confirmationPictureUrl)
        {
            var machineTask = await MachineTaskDao.Instance.GetMachineTask(machineTaskId);

            string oldStatus = machineTask.Status;
            if (confirmationPictureUrl != null && status == MachineTaskEnum.Completed.ToString())
            {
                machineTask.ConfirmationPictureUrl = confirmationPictureUrl;
            }

            machineTask.Status = status;

            if (status == MachineTaskEnum.Completed.ToString())
            {
                machineTask.DateCompleted = DateTime.Now;
            }

            await MachineTaskDao.Instance.UpdateAsync(machineTask);

            string action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<MachineTaskEnum>(oldStatus)}] trở thành [{EnumExtensions.TranslateStatus<MachineTaskEnum>(status)}]";

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
