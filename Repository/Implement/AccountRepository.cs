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

            //account.RoleId = (int)AccountRoleEnum.Customer;
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

            //Init promotion
            //var promotions = await PromotionDao.Instance.GetAllAsync();
            //var activePromotions = promotions.Where(p => p.Status!.Equals(PromotionStatusEnum.Active.ToString()));
            //if (!activePromotions.IsNullOrEmpty())
            //{
            //    foreach (var activePromotion in activePromotions)
            //    {
            //        var accountPromotion = new AccountPromotion
            //        {
            //            Account = account,
            //            DateReceive = DateTime.Now,
            //            Status = AccountPromotionStatusEnum.Active.ToString(),
            //            PromotionId = activePromotion.PromotionId,
            //        };

            //        account.AccountPromotions.Add(accountPromotion);
            //    }
            //}

            await AccountDao.Instance.CreateAsync(account);

            return _mapper.Map<AccountDto>(account);
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

        public async Task<int> UpdateEmployeeAccount(int accountId, EmployeeAccountUpdateDto employeeAccountUpdateDto)
        {
            var employeeAccount = await AccountDao.Instance.GetAccountAsyncById(accountId);

            if (employeeAccount != null)
            {
                employeeAccount.Name = employeeAccountUpdateDto.Name;
                employeeAccount.Email = employeeAccountUpdateDto.Email;
                employeeAccount.Phone = employeeAccountUpdateDto.Phone;
                employeeAccount.Username = employeeAccountUpdateDto.Username;
                employeeAccount.RoleId = employeeAccountUpdateDto.RoleId;
                employeeAccount.Gender = employeeAccountUpdateDto.Gender;
                employeeAccount.DateBirth = employeeAccountUpdateDto.DateBirth;
                employeeAccount.DateExpire = employeeAccountUpdateDto.DateExpire;

                await AccountDao.Instance.UpdateAsync(employeeAccount);

                return employeeAccount.AccountId;
            }

            return 0;
        }

        public async Task<bool> IsCustomerAccountValidToUpdate(int accountId, CustomerAccountUpdateDto customerAccountUpdateDto)
        {
            return await AccountDao.Instance.IsCustomerAccountValidToUpdate(accountId, customerAccountUpdateDto);
        }

        public async Task<int> UpdateCustomerAccount(int accountId, CustomerAccountUpdateDto customerAccountUpdateDto)
        {
            var customerAccount = await AccountDao.Instance.GetAccountAsyncById(accountId);

            if (customerAccount != null)
            {
                customerAccount.Name = customerAccountUpdateDto.Name;
                customerAccount.Email = customerAccountUpdateDto.Email;
                customerAccount.Phone = customerAccountUpdateDto.Phone;
                customerAccount.Gender = customerAccountUpdateDto.Gender;
                customerAccount.DateBirth = customerAccountUpdateDto.DateBirth;
                customerAccount.AvatarImg = customerAccountUpdateDto.AvatarImg;

                var accountBusiness = customerAccount.AccountBusiness;
                if (accountBusiness != null)
                {
                    accountBusiness.TaxNumber = customerAccountUpdateDto?.TaxNumber;
                    accountBusiness.Address = customerAccountUpdateDto?.Address;
                    accountBusiness.Company = customerAccountUpdateDto?.Company;
                    accountBusiness.Position = customerAccountUpdateDto?.Position;

                    customerAccount.AccountBusiness = accountBusiness;
                }

                await AccountDao.Instance.UpdateCustomerAccount(customerAccount);

                return customerAccount.AccountId;
            }

            return 0;
        }


    }
}
