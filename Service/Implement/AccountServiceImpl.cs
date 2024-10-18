using Common;
using Common.Enum;
using DTOs.Account;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.Mail;

namespace Service.Implement
{
    public class AccountServiceImpl : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMailService _mailService;

        public AccountServiceImpl(IAccountRepository accountRepository, IMailService mailService)
        {
            _accountRepository = accountRepository;
            _mailService = mailService;
        }

        public async Task ChangeAccountStatus(int accountId, string status)
        {
            await CheckAccountExist(accountId);

            if (!Enum.IsDefined(typeof(AccountStatusEnum), status))
            {
                throw new ServiceException(MessageConstant.InvalidStatusValue);
            }

            await _accountRepository.ChangeAccountStatus(accountId, status);
        }

        public async Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role)
        {
            if (role.HasValue && !Enum.IsDefined(typeof(AccountRoleEnum), role.Value))
            {
                throw new ServiceException(MessageConstant.Account.InvalidRoleValue);
            }

            return await _accountRepository.GetAccountsByRole(role);
        }

        public async Task<IEnumerable<EmployeeAccountDto>> GetEmployeeAccounts()
        {
            return await _accountRepository.GetEmployeeAccounts();
        }

        public async Task<CustomerAccountDetailDto> GetCustomerAccountById(int accountId)
        {
            var account = await CheckAccountExist(accountId);
            if (account.RoleId != (int)AccountRoleEnum.Customer)
            {
                throw new ServiceException(MessageConstant.Account.NotCustomerRole);
            }

            return await _accountRepository.GetCustomerAccountById(accountId);
        }

        public async Task<EmployeeAccountDto> GetEmployeeAccountById(int accountId)
        {
            var account = await CheckAccountExist(accountId);
            if (account.RoleId == (int)AccountRoleEnum.Customer)
            {
                throw new ServiceException(MessageConstant.Account.NotStaffOrManagerRole);
            }

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
            bool isExist = await _accountRepository.IsAccountExistWithEmail(newEmployeeAccountDto.Email);

            if (isExist)
            {
                throw new ServiceException(MessageConstant.Account.EmailAlreadyExists);
            }

            bool isUsernameExist = await _accountRepository.IsAccountExistWithUsername(newEmployeeAccountDto.Username);
            if (isUsernameExist)
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

        public async Task<AccountDto> GetAccount(int accountId)
        {
            var account = await _accountRepository.GetAccounById(accountId);

            return account;
        }

        public async Task<IEnumerable<EmployeeAccountDto>> GetStaffAccounts()
        {
            return await _accountRepository.GetStaffAccounts();
        }

        public async Task<CustomerAccountDto> GetCustomerAccount(int accountId)
        {
            var list = await _accountRepository.GetCustomerAccounts();
            return list.FirstOrDefault(a => a.AccountId == accountId);

        }

        public async Task<EmployeeAccountDto> GetEmployeeAccount(int accountId)
        {
            var list = await _accountRepository.GetEmployeeAccounts();
            return list.FirstOrDefault(a => a.AccountId == accountId);
        }

        public async Task<IEnumerable<StaffAccountDto>> GetActiveStaffAccounts()
        {
            return await _accountRepository.GetActiveStaffAccounts();
        }

        public async Task<int> UpdateEmployeeAccount(int accountId, EmployeeAccountUpdateDto employeeAccountUpdateDto)
        {
            var isValid = await _accountRepository.IsEmployeeAccountValidToUpdate(accountId, employeeAccountUpdateDto);
            if (!isValid)
            {
                throw new ServiceException(MessageConstant.Account.EmployeeAccountNotValidToUpdate);
            }

            return await _accountRepository.UpdateEmployeeAccount(accountId, employeeAccountUpdateDto);
        }
    }
}