using BusinessObject;
using Common.Enum;
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
                    query = query.Where(a => a.RoleId == roleId.Value);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<Account>> GetManagerAndStaffAccountsAsync()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Accounts.Where(a => a.RoleId != (int)AccountRoleEnum.Customer).ToListAsync();
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

        public async Task<IEnumerable<Account>> GetManagerAndStaffAccountsInRangeAsync(DateTime? startDate, DateTime? endDate)
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

    }
}
