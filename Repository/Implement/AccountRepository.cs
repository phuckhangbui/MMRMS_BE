using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.Account;
using DTOs.MembershipRank;
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

        public async Task<bool> ChangeAccountStatus(int acocuntId, string status)
        {
            var currentAccount = await AccountDao.Instance.GetAccountAsyncById(acocuntId);
            if (currentAccount != null)
            {
                currentAccount.Status = status;
                await AccountDao.Instance.UpdateAsync(currentAccount);

                return true;
            }

            return false;
        }

        public async Task<AccountDto> CreateCustomerAccount(NewCustomerAccountDto newCustomerAccountDto)
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
            account.AvatarImg = GlobalConstant.DefaultAvatarUrl;
            account.OtpNumber = otp;
            account.MoneySpent = 0;

            var customerRole = await RoleDao.Instance.GetRoleByRoleName(AccountRoleEnum.Customer);
            if (customerRole != null)
            {
                account.RoleId = customerRole.RoleId;
            }

            var accountBusiness = new AccountBusiness
            {
                Company = newCustomerAccountDto.Company,
                Position = newCustomerAccountDto.Position,
                TaxNumber = newCustomerAccountDto.TaxNumber,
                Address = newCustomerAccountDto.Address,
            };

            account.AccountBusiness = accountBusiness;

            var membershipRanks = await MembershipRankDao.Instance.GetAllAsync();
            var defaultMembershipRank = membershipRanks.OrderBy(m => m.MoneySpent).FirstOrDefault();
            if (defaultMembershipRank != null)
            {
                account.MembershipRankId = defaultMembershipRank.MembershipRankId;
            }

            var newAccount = await AccountDao.Instance.CreateAsync(account);

            if (defaultMembershipRank != null)
            {
                var memberhipRankLog = new MembershipRankLog
                {
                    MembershipRankId = defaultMembershipRank.MembershipRankId,
                    AccountId = newAccount.AccountId,
                    Action = $"{GlobalConstant.MembershipRankLogRankUpgradedAction}{defaultMembershipRank.MembershipRankName}",
                    DateCreate = DateTime.UtcNow,
                };

                await MembershipRankLogDao.Instance.CreateAsync(memberhipRankLog);

            }

            return _mapper.Map<AccountDto>(newAccount);
        }

        public async Task<EmployeeAccountDto> CreateEmployeeAccount(NewEmployeeAccountDto newEmployeeAccountDto)
        {

            var account = _mapper.Map<Account>(newEmployeeAccountDto);

            using var hmac = new HMACSHA512();
            account.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(GlobalConstant.DefaultPassword));
            account.PasswordSalt = hmac.Key;
            account.DateCreate = DateTime.Now;
            account.Status = AccountStatusEnum.Active.ToString();
            account.IsDelete = false;
            account.AvatarImg = GlobalConstant.DefaultAvatarUrl;
            account.RoleId = newEmployeeAccountDto.RoleId;

            account = await AccountDao.Instance.CreateAsync(account);

            return _mapper.Map<EmployeeAccountDto>(account);
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

        public async Task<CustomerAccountDetailDto> GetCustomerAccountById(int accountId)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);
            var customerAccountDetailDto = _mapper.Map<CustomerAccountDetailDto>(account);

            var membershipRank = await MembershipRankDao.Instance.GetMembershipRanksForCustomer(accountId);
            if (membershipRank != null)
            {
                customerAccountDetailDto.MembershipRank = _mapper.Map<MembershipRankDto>(membershipRank);
            }

            return customerAccountDetailDto;
        }

        public async Task<IEnumerable<CustomerAccountDto>> GetCustomerAccounts()
        {
            var accounts = await AccountDao.Instance.GetAccountsByRoleAsync((int)AccountRoleEnum.Customer);

            return _mapper.Map<IEnumerable<CustomerAccountDto>>(accounts);
        }

        public async Task<IEnumerable<EmployeeAccountDto>> GetEmployeeAccounts()
        {
            var accounts = await AccountDao.Instance.GetEmployeeAccountsAsync();
            return _mapper.Map<IEnumerable<EmployeeAccountDto>>(accounts);
        }

        public async Task<EmployeeAccountDto> GetEmployeeAccountById(int accountId)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);
            return _mapper.Map<EmployeeAccountDto>(account);
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

        public async Task<AccountDto> FirebaseTokenExisted(string firebaseToken)
        {
            var list = await AccountDao.Instance.GetAllAsync();
            var account = list.FirstOrDefault(x => x.FirebaseMessageToken == firebaseToken);
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<IEnumerable<AccountDto>> GetManagerAccounts()
        {
            var list = await AccountDao.Instance.GetAllAsync();
            var managerAccounts = list.Where(x => x.RoleId == (int)AccountRoleEnum.Manager).ToList();
            return _mapper.Map<IEnumerable<AccountDto>>(managerAccounts);
        }

        public async Task<IEnumerable<EmployeeAccountDto>> GetStaffAccounts()
        {
            var accounts = await AccountDao.Instance.GetStaffAccountsAsync();
            return _mapper.Map<IEnumerable<EmployeeAccountDto>>(accounts);
        }

        public async Task<IEnumerable<StaffAccountDto>> GetActiveStaffAccounts()
        {
            var accounts = await AccountDao.Instance.GetStaffAccountsAsync();

            accounts = accounts
                .Where(a => a.Status.Equals(AccountStatusEnum.Active.ToString()) && a.RoleId == (int)AccountRoleEnum.TechnicalStaff)
                .ToList();

            return _mapper.Map<IEnumerable<StaffAccountDto>>(accounts);
        }

        public async Task<bool> IsEmployeeAccountValidToUpdate(int accountId, EmployeeAccountUpdateDto employeeAccountUpdateDto)
        {
            return await AccountDao.Instance.IsEmployeeAccountValidToUpdate(accountId, employeeAccountUpdateDto);
        }

        public async Task<bool> IsAccountValidToUpdate(int accountId, string email, string phone)
        {
            return await AccountDao.Instance.IsAccountValidToUpdate(accountId, email, phone);
        }

        public async Task<int> UpdateAccount(int accountId, IAccountUpdateDto accountUpdateDto)
        {
            var account = await AccountDao.Instance.GetAccountAsyncById(accountId);
            if (account == null)
            {
                return 0;
            }

            account.Name = accountUpdateDto.Name;
            account.Email = accountUpdateDto.Email;
            account.Phone = accountUpdateDto.Phone;
            account.Gender = accountUpdateDto.Gender;
            account.DateBirth = accountUpdateDto.DateBirth;

            if (accountUpdateDto is EmployeeAccountUpdateDto employeeDto)
            {
                account.Username = employeeDto.Username;
                account.RoleId = employeeDto.RoleId;
                account.DateExpire = employeeDto.DateExpire;
                await AccountDao.Instance.UpdateAsync(account);
            }
            else if (accountUpdateDto is CustomerAccountUpdateDto customerDto)
            {
                account.AvatarImg = customerDto.AvatarImg;

                if (account.AccountBusiness != null)
                {
                    account.AccountBusiness.TaxNumber = customerDto.TaxNumber;
                    account.AccountBusiness.Address = customerDto.Address;
                    account.AccountBusiness.Company = customerDto.Company;
                    account.AccountBusiness.Position = customerDto.Position;
                }

                await AccountDao.Instance.UpdateCustomerAccount(account);
            }
            else if (accountUpdateDto is EmployeeProfileUpdateDto)
            {
                await AccountDao.Instance.UpdateAsync(account);
            }

            return account.AccountId;
        }
    }
}
