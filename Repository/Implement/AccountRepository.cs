using AutoMapper;
using BusinessObject;
using Common;
using DAO;
using DAO.Enum;
using DTOs.Account;
using Repository.Interface;
using System.Security.Cryptography;
using System.Text;
using Account = BusinessObject.Account;

namespace Repository.Implement
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMapper _mapper;

        public AccountRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task ChangeAccountStatus(int acocuntId, string status)
        {
            var currentAccount = await AccountDao.Instance.GetAccountAsyncById(acocuntId);
            if (currentAccount != null)
            {
                currentAccount.Status = status;
                await AccountDao.Instance.UpdateAsync(currentAccount);
            }
        }

        public async Task CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto)
        {
            Random random = new Random();
            var otp = random.Next(111111, 999999).ToString();
            var account = _mapper.Map<Account>(newCustomerAccountDto);

            using var hmac = new HMACSHA512();
            account.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newCustomerAccountDto.Password));
            account.PasswordSalt = hmac.Key;
            account.DateCreate = DateTime.Now;
            account.Status = AccountStatusEnum.Inactive.ToString();
            account.IsDelete = false;
            account.AvatarImg = GlobalConstants.DefaultAvatarUrl;
            account.OtpNumber = otp;

            account.RoleId = (int)AccountRoleEnum.Customer;
            account.BusinessType = newCustomerAccountDto.BusinessType;
            var accountBusiness = new AccountBusiness
            {
                Company = newCustomerAccountDto.Company,
                Position = newCustomerAccountDto.Position,
                TaxNumber = newCustomerAccountDto.TaxNumber,
                Address = newCustomerAccountDto.Address,
            };

            account.AccountBusinesses.Add(accountBusiness);

            await AccountDao.Instance.CreateAsync(account);


        }

        public async Task CreateStaffOrManagerAccount(NewStaffAndManagerAccountDto newStaffAndManagerAccountDto)
        {

            var account = _mapper.Map<Account>(newStaffAndManagerAccountDto);

            using var hmac = new HMACSHA512();
            account.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(GlobalConstants.DefaultPassword));
            account.PasswordSalt = hmac.Key;
            account.DateCreate = DateTime.Now;
            account.Status = AccountStatusEnum.Inactive.ToString();
            account.IsDelete = false;
            account.AvatarImg = GlobalConstants.DefaultAvatarUrl;

            await AccountDao.Instance.CreateAsync(account);
        }

        public async Task<AccountBaseDto> GetAccountBaseById(int accountId)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);

            return _mapper.Map<AccountBaseDto>(account);
        }

        public async Task<AccountDto> GetAccounById(int accountId)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);

            return _mapper.Map<AccountDto>(account);
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

        public async Task<CustomerAccountDto> GetCustomerAccountById(int accountId)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);
            return _mapper.Map<CustomerAccountDto>(account);
        }

        public async Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts()
        {
            var accounts = await AccountDao.Instance.GetAccountsByRoleAsync((int)AccountRoleEnum.Customer);

            return _mapper.Map<IEnumerable<CustomerAccountDto>>(accounts);
        }

        public async Task<IEnumerable<StaffAndManagerAccountDto>> GetManagerAndStaffAccountsByRole()
        {
            var accounts = await AccountDao.Instance.GetManagerAndStaffAccountsAsync();
            return _mapper.Map<IEnumerable<StaffAndManagerAccountDto>>(accounts);
        }

        public async Task<StaffAndManagerAccountDto> GetStaffAndManagerAccountById(int accountId)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);
            return _mapper.Map<StaffAndManagerAccountDto>(account);
        }

        public async Task<bool> IsAccountExistWithEmail(string email)
        {
            return await AccountDao.Instance.IsAccountExistWithEmailAsync(email);
        }

        public async Task<bool> IsAccountExistWithUsername(string username)
        {
            return await AccountDao.Instance.IsAccountExistWithUsernameAsync(username);
        }

        public async Task<AccountDto> GetCustomerAccountWithEmail(string email)
        {
            var account = await AccountDao.Instance.GetAccountByEmail(email);
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> GetStaffAndManagerAccountWithUsername(string username)
        {
            var account = await AccountDao.Instance.GetAccountByUsername(username);
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> GetAccountDtoById(int accountId)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);

            return _mapper.Map<AccountDto>(account);
        }

        public async Task UpdateAccount(AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);

            await AccountDao.Instance.UpdateAsync(account);
        }

        public async Task ChangeAccountPassword(AccountDto accountDto, string password)
        {
            using var hmac = new HMACSHA512();
            accountDto.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            accountDto.PasswordSalt = hmac.Key;

            var account = _mapper.Map<Account>(accountDto);

            await AccountDao.Instance.UpdateAsync(account);
        }

    }
}
