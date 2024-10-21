using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
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

        public async Task CreateMachineTaskWithRequest(int managerId, CreateMachineTaskCheckMachineDto createMachineTaskDto)
        {
            var staffAccount = await _accountRepository.GetAccounById(createMachineTaskDto.StaffId);

            var request = await MachineCheckRequestDao.Instance.GetMachineCheckRequest(createMachineTaskDto.RequestId);

            var now = DateTime.Now;
            var requestResponse = new RequestResponse
            {
                MachineCheckRequestId = createMachineTaskDto.RequestId,
                DateCreate = now,
                DateResponse = createMachineTaskDto.DateStart,
                Action = $"Yêu cầu của bạn đã được giao cho một nhân viên kiểm tra"
            };

            requestResponse = await RequestResponseDao.Instance.CreateAsync(requestResponse);

            var task = new MachineTask
            {
                TaskTitle = createMachineTaskDto.TaskTitle,
                Content = createMachineTaskDto.TaskContent,
                StaffId = createMachineTaskDto.StaffId,
                ManagerId = managerId,
                Type = MachineTaskTypeEnum.CheckMachinery.ToString(),
                DateCreate = now,
                DateStart = createMachineTaskDto.DateStart,
                Status = MachineTaskStatusEnum.Assigned.ToString(),
                Note = createMachineTaskDto.Note,
                RequestResponseId = requestResponse.RequestResponseId,
                ContractId = request.ContractId
            };

            var taskLog = new MachineTaskLog
            {
                MachineTaskId = task.MachineTaskId,
                Action = $"Create and assign task to staff name {staffAccount.Name}",
                DateCreate = now,
                AccountTriggerId = managerId,
            };

            task = await MachineTaskDao.Instance.CreateAsync(task);

            await MachineTaskLogDao.Instance.CreateAsync(taskLog);

            requestResponse.MachineTaskId = task.MachineTaskId;
            await RequestResponseDao.Instance.UpdateAsync(requestResponse);

            request.Status = MachineCheckRequestStatusEnum.Assigned.ToString();
            await MachineCheckRequestDao.Instance.UpdateAsync(request);
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
            var MachineTask = await MachineTaskDao.Instance.GetMachineTaskDetail(taskId);

            var taskDetail = _mapper.Map<MachineTaskDisplayDetail>(MachineTask);

            return taskDetail;
        }

        public async Task<IEnumerable<MachineTaskDto>> GetMachineTasks()
        {
            var list = await MachineTaskDao.Instance.GetMachineTasks();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(list);
        }

        public async Task<IEnumerable<MachineTaskDto>> GetTaskOfStaffInADay(int staffId, DateTime date)
        {
            var list = await MachineTaskDao.Instance.GetMachineTasks();

            var staffList = list.Where(t => t.StaffId == staffId).ToList();

            var filteredList = staffList.Where(d => d.DateStart.HasValue && d.DateStart.Value.Date == date.Date).ToList();

            return _mapper.Map<IEnumerable<MachineTaskDto>>(filteredList);
        }

        public async Task UpdateTaskStatus(int MachineTaskId, string status, int accountId)
        {
            var MachineTask = await MachineTaskDao.Instance.GetMachineTask(MachineTaskId);

            string oldStatus = MachineTask.Status;

            MachineTask.Status = status;

            await MachineTaskDao.Instance.UpdateAsync(MachineTask);

            string action = $"Change status from {oldStatus} to {status}";

            var taskLog = new MachineTaskLog
            {
                MachineTaskId = MachineTaskId,
                AccountTriggerId = accountId,
                DateCreate = DateTime.Now,
                Action = action,
            };

            await MachineTaskLogDao.Instance.CreateAsync(taskLog);
        }
    }
}
