﻿using AutoMapper;
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
                RequestResponseId = requestResponse.RequestResponseId,
                ContractId = request.ContractId
            };

            var taskLog = new MachineTaskLog
            {
                Action = $"Tạo và giao công việc kiểm tra máy cho {staffAccount.Name}",
                DateCreate = now,
                AccountTriggerId = managerId,
            };

            await MachineTaskDao.Instance.CreateMachineTaskBaseOnRequest(task, taskLog, requestResponse);

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
            var machineTask = await MachineTaskDao.Instance.GetMachineTaskDetail(taskId);

            List<MachineTaskLog> machineTaskLogs = (List<MachineTaskLog>)machineTask.MachineTaskLogs;

            var taskLogsDto = _mapper.Map<List<MachineTaskLogDto>>(machineTaskLogs);

            //var taskLogsto = new List<MachineTaskLogDto>();

            //foreach (var log in machineTaskLogs)
            //{
            //    var logDto = _mapper.Map<MachineTaskLogDto>(log);
            //    taskLogsto.Add(logDto);
            //}

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

        public async Task<IEnumerable<MachineTaskDto>> GetTaskOfStaffInADay(int staffId, DateTime date)
        {
            var list = await MachineTaskDao.Instance.GetMachineTasks();

            var staffList = list.Where(t => t.StaffId == staffId).ToList();

            var filteredList = staffList.Where(d => d.DateStart.HasValue && d.DateStart.Value.Date == date.Date).ToList();

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
