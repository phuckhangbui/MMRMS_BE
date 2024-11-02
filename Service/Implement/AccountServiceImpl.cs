using Common;
using Common.Enum;
using DTOs.Account;
using DTOs.MachineTask;
using Microsoft.Extensions.Configuration;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.Mail;

namespace Service.Implement
{
    public class AccountServiceImpl : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IMailService _mailService;
        private readonly IDeliveryTaskRepository _deliveryTaskRepository;
        private readonly IMachineTaskRepository _machineTaskRepository;

        public AccountServiceImpl(IConfiguration configuration, IAccountRepository accountRepository, IMailService mailService, IDeliveryTaskRepository deliveryTaskRepository, IMachineTaskRepository machineTaskRepository)
        {
            _accountRepository = accountRepository;
            _mailService = mailService;
            _configuration = configuration;
            _deliveryTaskRepository = deliveryTaskRepository;
            _machineTaskRepository = machineTaskRepository;
        }

        public async Task<bool> ChangeAccountStatus(int accountId, string status)
        {
            await CheckAccountExist(accountId);

            if (!Enum.IsDefined(typeof(AccountStatusEnum), status))
            {
                throw new ServiceException(MessageConstant.InvalidStatusValue);
            }

            return await _accountRepository.ChangeAccountStatus(accountId, status);
        }

        public async Task<IEnumerable<EmployeeAccountDto>> GetEmployeeAccounts()
        {
            return await _accountRepository.GetEmployeeAccounts();
        }

        public async Task<CustomerAccountDetailDto> GetCustomerAccountDetail(int accountId)
        {
            await CheckAccountExist(accountId);

            return await _accountRepository.GetCustomerAccountById(accountId);
        }

        public async Task<EmployeeAccountDto> GetEmployeeAccountDetail(int accountId)
        {
            await CheckAccountExist(accountId);

            return await _accountRepository.GetEmployeeAccountById(accountId);
        }

        private async Task<AccountBaseDto> CheckAccountExist(int accountId)
        {
            var account = await _accountRepository.GetAccountBaseById(accountId);
            if (account == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            return account;
        }

        public async Task<int> CreateEmployeeAccount(NewEmployeeAccountDto newEmployeeAccountDto)
        {
            var adminUsername = _configuration.GetSection("AdminAccount:Username").Value;

            bool isExist = await _accountRepository.IsAccountExistWithEmail(newEmployeeAccountDto.Email);

            if (isExist)
            {
                throw new ServiceException(MessageConstant.Account.EmailAlreadyExists);
            }

            bool isUsernameExist = await _accountRepository.IsAccountExistWithUsername(newEmployeeAccountDto.Username);
            if (isUsernameExist || adminUsername.Equals(newEmployeeAccountDto.Username))
            {
                throw new ServiceException(MessageConstant.Account.UsernameAlreadyExists);
            }

            var accountDto = await _accountRepository.CreateEmployeeAccount(newEmployeeAccountDto);

            //Send mail
            _mailService.SendMail(AuthenticationMail.SendWelcomeAndCredentialsToEmployee(accountDto.Email, accountDto.Name, accountDto.Username, GlobalConstant.DefaultPassword));

            return accountDto.AccountId;
        }

        public async Task CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto)
        {
            bool isExist = await _accountRepository.IsAccountExistWithEmail(newCustomerAccountDto.Email);

            if (isExist)
            {
                throw new ServiceException(MessageConstant.Account.EmailAlreadyExists);
            }

            await _accountRepository.CreateCustomerAccount(newCustomerAccountDto);
        }

        public async Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts()
        {
            return await _accountRepository.GetCustomerAccounts();
        }

        public async Task<IEnumerable<EmployeeAccountDto>> GetStaffAccounts()
        {
            return await _accountRepository.GetStaffAccounts();
        }

        public async Task<IEnumerable<StaffAccountDto>> GetActiveStaffAccounts()
        {
            return await _accountRepository.GetActiveStaffAccounts();
        }

        public async Task<int> UpdateEmployeeAccount(int accountId, EmployeeAccountUpdateDto employeeAccountUpdateDto)
        {
            var allowedRoles = new List<AccountRoleEnum>
            {
                AccountRoleEnum.Manager,
                AccountRoleEnum.TechnicalStaff,
                AccountRoleEnum.WebsiteStaff
            };

            if (!allowedRoles.Contains((AccountRoleEnum)employeeAccountUpdateDto.RoleId) || !Enum.IsDefined(typeof(AccountRoleEnum), employeeAccountUpdateDto.RoleId))
            {
                throw new ServiceException(MessageConstant.Account.InvalidRoleValue);
            }

            var isValid = await _accountRepository.IsEmployeeAccountValidToUpdate(accountId, employeeAccountUpdateDto);
            if (!isValid)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotValidToUpdate);
            }

            return await _accountRepository.UpdateAccount(accountId, employeeAccountUpdateDto);
        }

        public async Task<int> UpdateCustomerAccount(int accountId, CustomerAccountUpdateDto customerAccountUpdateDto)
        {
            var isValid = await _accountRepository.IsAccountValidToUpdate(accountId, customerAccountUpdateDto.Email, customerAccountUpdateDto.Phone);
            if (!isValid)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotValidToUpdate);
            }

            return await _accountRepository.UpdateAccount(accountId, customerAccountUpdateDto);
        }

        public async Task<IEnumerable<TaskAndDeliveryScheduleDto>> GetStaffSchedule(int staffId, DateOnly dateStart, DateOnly dateEnd)
        {
            var staff = await _accountRepository.GetAccounById(staffId);

            if (staff.RoleId != (int)AccountRoleEnum.TechnicalStaff)
            {
                throw new ServiceException(MessageConstant.Account.InvalidRoleValue);
            }

            var machineTaskList = await _machineTaskRepository.GetMachineTasksForStaff(staffId, dateStart, dateEnd);

            var deliveryList = await _deliveryTaskRepository.GetDeliveryTasksForStaff(staffId, dateStart, dateEnd);

            var taskAndDeliveryList = new List<TaskAndDeliveryScheduleDto>();

            foreach (var task in machineTaskList)
            {
                var schedule = new TaskAndDeliveryScheduleDto
                {
                    StaffId = staffId,
                    StaffName = staff.Name,
                    MachineTaskId = task.MachineTaskId,
                    Type = TaskAndDeliveryScheduleDtoTypeEnum.MachineTask.ToString(),
                    Status = task.Status,
                    DateStart = DateOnly.FromDateTime((DateTime)task.DateStart),
                };

                taskAndDeliveryList.Add(schedule);
            }

            foreach (var delivery in deliveryList)
            {
                var schedule = new TaskAndDeliveryScheduleDto
                {
                    StaffId = staffId,
                    StaffName = staff.Name,
                    DeliveryTaskId = delivery.DeliveryTaskId,
                    Type = TaskAndDeliveryScheduleDtoTypeEnum.DeliveryTask.ToString(),
                    Status = delivery.Status,
                    DateStart = DateOnly.FromDateTime((DateTime)delivery.DateShip),
                };

                taskAndDeliveryList.Add(schedule);
            }

            return taskAndDeliveryList.OrderBy(s => s.DateStart);
        }

        public async Task<IEnumerable<TaskAndDeliveryScheduleDto>> GetStaffSchedule(DateOnly dateStart, DateOnly dateEnd)
        {
            var staffList = await _accountRepository.GetActiveStaffAccounts();

            var taskAndDeliveryList = new List<TaskAndDeliveryScheduleDto>();

            foreach (var staff in staffList)
            {
                var schedule = await GetStaffSchedule(staff.AccountId, dateStart, dateEnd);

                taskAndDeliveryList.AddRange(schedule);
            }

            return taskAndDeliveryList.OrderBy(s => s.DateStart);
        }

        public async Task<IEnumerable<StaffScheduleCounterDto>> GetStaffScheduleFromADate(DateOnly date)
        {
            var machineTaskDateList = await _machineTaskRepository.GetMachineTasksInADate(date);
            var deliveryTaskDateList = await _deliveryTaskRepository.GetDeliveryTasksInADate(date);

            var taskAndDeliveryList = new List<TaskAndDeliveryScheduleDto>();

            foreach (var task in machineTaskDateList)
            {
                var schedule = new TaskAndDeliveryScheduleDto
                {
                    StaffId = task.StaffId,
                    MachineTaskId = task.MachineTaskId,
                    Type = TaskAndDeliveryScheduleDtoTypeEnum.MachineTask.ToString(),
                    Status = task.Status,
                    DateStart = DateOnly.FromDateTime((DateTime)task.DateStart)
                };
                taskAndDeliveryList.Add(schedule);
            }

            foreach (var delivery in deliveryTaskDateList)
            {
                var schedule = new TaskAndDeliveryScheduleDto
                {
                    StaffId = delivery.StaffId,
                    DeliveryTaskId = delivery.DeliveryTaskId,
                    Type = TaskAndDeliveryScheduleDtoTypeEnum.DeliveryTask.ToString(),
                    Status = delivery.Status,
                    DateStart = DateOnly.FromDateTime((DateTime)delivery.DateShip)
                };
                taskAndDeliveryList.Add(schedule);
            }

            var staffList = await _accountRepository.GetActiveStaffAccounts();

            var staffNames = staffList.ToDictionary(s => s.AccountId, s => s.Name);

            var staffScheduleCounters = staffList.Select(staff => new StaffScheduleCounterDto
            {
                StaffId = staff.AccountId,
                StaffName = staffNames[staff.AccountId],
                DateStart = date,
                TaskCounter = taskAndDeliveryList.Count(t => t.StaffId == staff.AccountId),
                CanReceiveMoreTask = taskAndDeliveryList.Count(t => t.StaffId == staff.AccountId) < GlobalConstant.MaxTaskLimitADay,
                TaskAndDeliverySchedules = taskAndDeliveryList
                    .Where(t => t.StaffId == staff.AccountId)
                    .OrderBy(t => t.DateStart)
                    .ToList()
            });

            return staffScheduleCounters;
        }

        public async Task<int> UpdateEmployeeProfile(int accountId, EmployeeProfileUpdateDto employeeProfileUpdateDto)
        {
            var isValid = await _accountRepository.IsAccountValidToUpdate(accountId, employeeProfileUpdateDto.Email, employeeProfileUpdateDto.Phone);
            if (!isValid)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotValidToUpdate);
            }

            return await _accountRepository.UpdateAccount(accountId, employeeProfileUpdateDto);
        }
    }
}