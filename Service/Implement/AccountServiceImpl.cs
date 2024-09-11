using BusinessObject;
using DAO.Enum;
using DTOs.Account;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class AccountServiceImpl : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountServiceImpl(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task ChangeAccountStatus(int accountId, int status)
        {
            await CheckAccountExist(accountId);

            if (!Enum.IsDefined(typeof(AccountStatusEnum), status))
            {
                throw new ServiceException("Invalid status value");
            }

            await _accountRepository.ChangeAccountStatus(accountId, status);
        }

        public async Task CreateAccount(NewCustomerAccountDto? newCustomerAccountDto, NewStaffAndManagerAccountDto? newStaffAndManagerAccountDto)
        {
            var email = newCustomerAccountDto?.Email ?? newStaffAndManagerAccountDto?.Email;

            bool isExist = await _accountRepository.IsAccountExistWithEmail(email!);

            if (isExist)
            {
                throw new ServiceException("An account with this email already exists.");
            }

            if (newCustomerAccountDto != null)
            {
                await _accountRepository.CreateAccount(newCustomerAccountDto, null);
            }
            else if (newStaffAndManagerAccountDto != null)
            {
                bool isUsernameExist = await _accountRepository.IsAccountExistWithUsername(newStaffAndManagerAccountDto.Username);
                if (isUsernameExist)
                {
                    throw new ServiceException("An account with this username already exists.");
                }

                await _accountRepository.CreateAccount(null, newStaffAndManagerAccountDto);
            }
            else
            {
                throw new ServiceException("No valid account data provided.");
            }
        }

        public async Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role)
        {
            if (role.HasValue && !Enum.IsDefined(typeof(AccountRoleEnum), role.Value))
            {
                throw new ServiceException("Invalid role value");
            }

            return await _accountRepository.GetAccountsByRole(role);
        }

        public async Task<IEnumerable<StaffAndManagerAccountDto>> GetManagerAndStaffAccountsByRole()
        {
            return await _accountRepository.GetManagerAndStaffAccountsByRole();
        }

        public async Task<CustomerAccountDto> GetCustomerAccountById(int accountId)
        {
            var account = await CheckAccountExist(accountId);
            if (account.RoleID != (int)AccountRoleEnum.Customer)
            {
                throw new ServiceException("This account is not role customer");
            }

            return await _accountRepository.GetCustomerAccountById(accountId);
        }

        public async Task<StaffAndManagerAccountDto> GetStaffAndManagerAccountById(int accountId)
        {
            var account = await CheckAccountExist(accountId);
            if (account.RoleID == (int)AccountRoleEnum.Customer)
            {
                throw new ServiceException("This account is not role staff or manager");
            }

            return await _accountRepository.GetStaffAndManagerAccountById(accountId);
        }


        private async Task<AccountBaseDto> CheckAccountExist(int accountId)
        {
            var account = await _accountRepository.GetAccountById(accountId);
            if (account == null)
            {
                throw new ServiceException("Account not found");
            }

            return account;
        }
    }
}