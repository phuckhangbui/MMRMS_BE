using Common;
using Common.Enum;
using DTOs.Account;
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

        public AccountServiceImpl(IConfiguration configuration, IAccountRepository accountRepository, IMailService mailService)
        {
            _accountRepository = accountRepository;
            _mailService = mailService;
            _configuration = configuration;
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
            var isValid = await _accountRepository.IsEmployeeAccountValidToUpdate(accountId, employeeAccountUpdateDto);
            if (!isValid)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotValidToUpdate);
            }

            return await _accountRepository.UpdateEmployeeAccount(accountId, employeeAccountUpdateDto);
        }

        public async Task<int> UpdateCustomerAccount(int accountId, CustomerAccountUpdateDto customerAccountUpdateDto)
        {
            var isValid = await _accountRepository.IsCustomerAccountValidToUpdate(accountId, customerAccountUpdateDto);
            if (!isValid)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotValidToUpdate);
            }

            return await _accountRepository.UpdateCustomerAccount(accountId, customerAccountUpdateDto);
        }
    }
}