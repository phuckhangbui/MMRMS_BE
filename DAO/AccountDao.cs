using BusinessObject;
using Common.Enum;
using DTOs.Account;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class AccountDao : BaseDao<Account>
    {
        private static AccountDao instance = null;
        private static readonly object instacelock = new object();

        private AccountDao()
        {

        }

        public static AccountDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<Account>> GetAccountsByRoleAsync(int? roleId)
        {
            using (var context = new MmrmsContext())
            {
                IQueryable<Account> query = context.Accounts;

                if (roleId.HasValue)
                {
                    query = query.Include(a => a.AccountBusiness).Where(a => a.RoleId == roleId.Value);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<Account>> GetEmployeeAccountsAsync()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Accounts.Where(a => a.RoleId != (int)AccountRoleEnum.Customer).ToListAsync();
            }
        }

        public async Task<IEnumerable<Account>> GetStaffAccountsAsync()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Accounts.Where(a => a.RoleId == (int)AccountRoleEnum.TechnicalStaff || a.RoleId == (int)AccountRoleEnum.WebsiteStaff).ToListAsync();
            }
        }

        public async Task<bool> IsAccountExistWithEmailAsync(string email)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Accounts.AnyAsync(a => a.Email.Equals(email));
            }
        }

        public async Task<Account> GetAccountAsyncById(int accountId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Accounts
                    .Include(a => a.AccountBusiness)
                    .FirstOrDefaultAsync(a => a.AccountId == accountId);
            }
        }

        public async Task<bool> IsAccountExistWithUsernameAsync(string username)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Accounts.AnyAsync(a => a.Username.Equals(username));
            }
        }


        public async Task<Account?> GetAccountByEmail(string email)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Accounts
                    .FirstOrDefaultAsync(a => !string.IsNullOrEmpty(a.Email) && a.Email == email);
            }
        }

        public async Task<Account?> GetAccountByUsername(string username)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Accounts
                    .FirstOrDefaultAsync(a => !string.IsNullOrEmpty(a.Username) && a.Username == username);
            }
        }

        public async Task<IEnumerable<Account>> GetAccountsByRoleInRangeAsync(int? roleId, DateTime? startDate, DateTime? endDate)
        {
            using (var context = new MmrmsContext())
            {
                IQueryable<Account> query = context.Accounts;

                if (roleId.HasValue)
                {
                    query = query.Where(a => a.RoleId == roleId.Value);
                }

                if (startDate.HasValue)
                {
                    query = query.Where(a => a.DateCreate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(a => a.DateCreate <= endDate.Value);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<Account>> GetEmployeeAccountsInRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            using (var context = new MmrmsContext())
            {
                IQueryable<Account> query = context.Accounts.Where(a => a.RoleId != (int)AccountRoleEnum.Customer);

                if (startDate.HasValue)
                {
                    query = query.Where(a => a.DateCreate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(a => a.DateCreate <= endDate.Value);
                }

                return await query.ToListAsync();
            }
        }

        //public async Task<bool> IsEmployeeAccountValidToUpdate(int accountId, EmployeeAccountUpdateDto updateDto)
        //{
        //    using (var context = new MmrmsContext())
        //    {
        //        var account = await context.Accounts.FindAsync(accountId);
        //        if (account == null || account.IsDelete == true || account.Status.Equals(AccountStatusEnum.Active))
        //        {
        //            return false;
        //        }

        //        bool emailExists = await context.Accounts
        //            .AnyAsync(a => a.Email == updateDto.Email && a.AccountId != accountId && a.IsDelete == false);

        //        bool phoneExists = await context.Accounts
        //            .AnyAsync(a => a.Phone == updateDto.Phone && a.AccountId != accountId && a.IsDelete == false);

        //        bool usernameExists = await context.Accounts
        //            .AnyAsync(a => a.Username == updateDto.Username && a.AccountId != accountId && a.IsDelete == false);

        //        if (emailExists || phoneExists || usernameExists)
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //}

        public async Task<bool> IsAccountValidToUpdate(int accountId, string email, string phone)
        {
            using (var context = new MmrmsContext())
            {
                var account = await context.Accounts.FindAsync(accountId);
                if (account == null || account.IsDelete == true || account.Status.Equals(AccountStatusEnum.Active))
                {
                    return false;
                }

                bool emailExists = await context.Accounts
                    .AnyAsync(a => a.Email == email && a.AccountId != accountId && a.IsDelete == false);

                bool phoneExists = await context.Accounts
                    .AnyAsync(a => a.Phone == phone && a.AccountId != accountId && a.IsDelete == false);

                if (emailExists || phoneExists)
                {
                    return false;
                }

                return true;
            }
        }

        public async Task<Account> UpdateCustomerAccount(Account account)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.AccountBusinesses.Update(account.AccountBusiness);

                        context.Accounts.Update(account);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return account;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
    }
}
