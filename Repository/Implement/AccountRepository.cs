using AutoMapper;
using BusinessObject;
using Common;
using DAO;
using DAO.Enum;
using DTOs.Account;
using Repository.Interface;
using System.Text;

namespace Repository.Implement
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMapper _mapper;

        public AccountRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task ChangeAccountStatus(int acocuntId, int status)
        {
            var currentAccount = await AccountDao.Instance.GetAccountAsyncById(acocuntId);
            if (currentAccount != null)
            {
                currentAccount.Status = status;
                await AccountDao.Instance.UpdateAsync(currentAccount);
            }
        }

        public async Task CreateAccount(NewCustomerAccountDto? newCustomerAccountDto, NewStaffAndManagerAccountDto? newStaffAndManagerAccountDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(GlobalConstants.DefaultPassword);

            var account = new Account();
            if (newCustomerAccountDto != null)
            {
                account = _mapper.Map<Account>(newCustomerAccountDto);
            }
            else if (newStaffAndManagerAccountDto != null)
            {
                account = _mapper.Map<Account>(newStaffAndManagerAccountDto);
            }

            account.PasswordHash = Encoding.UTF8.GetBytes(hashedPassword);
            account.DateCreate = DateTime.Now;
            account.Status = (int)AccountStatusEnum.Inactive;
            account.IsDelete = false;
            account.AvatarImg = GlobalConstants.DefaultAvatarUrl;

            await AccountDao.Instance.CreateAsync(account);
        }

        public Task CreateAccount(NewBaseAccountDto newAccountDto)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountBaseDto> GetAccountById(int accountId)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);

            return _mapper.Map<AccountBaseDto>(account);
        }

        public async Task<IEnumerable<AccountBaseDto>> GetAccountsByRole(int? role)
        {
            var accounts = await AccountDao.Instance.GetAccountsByRoleAsync(role);

            if (role.HasValue && role.Value == (int)AccountRoleEnum.Customer)
            {
                return _mapper.Map<IEnumerable<CustomerAccountDto>>(accounts);
            }
            else
            {
                return _mapper.Map<IEnumerable<StaffAndManagerAccountDto>>(accounts);
            }
        }

        public async Task<IEnumerable<StaffAndManagerAccountDto>> GetManagerAndStaffAccountsByRole()
        {
            var accounts = await AccountDao.Instance.GetManagerAndStaffAccountsAsync();
            return _mapper.Map<IEnumerable<StaffAndManagerAccountDto>>(accounts);
        }

        public async Task<bool> IsAccountExistWithEmail(string email)
        {
            return await AccountDao.Instance.IsAccountExistWithEmailAsync(email);
        }
    }
}
