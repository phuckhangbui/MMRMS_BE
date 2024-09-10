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
            var account = await _accountRepository.GetAccountById(accountId);
            if (account == null)
            {
                throw new ServiceException("Account not found");
            }

            if (!Enum.IsDefined(typeof(AccountStatusEnum), status))
            {
                throw new ServiceException("Invalid status value");
            }

            await _accountRepository.ChangeAccountStatus(accountId, status);
        }

        public async Task CreateAccount(NewCustomerAccountDto? newCustomerAccountDto, NewStaffAndManagerAccountDto? newStaffAndManagerAccountDto)
        {
            var email = newCustomerAccountDto?.Email ?? newStaffAndManagerAccountDto?.Email;

            if (email == null)
            {
                throw new ServiceException("No account data provided.");
            }

            bool isExist = await _accountRepository.IsAccountExistWithEmail(email);

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
    }
}