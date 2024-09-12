using BusinessObject;
using DAO.Enum;
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
            using (var context = new SmrmsContext())
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
            using (var context = new SmrmsContext())
            {
                return await context.Accounts.Where(a => a.RoleId != (int)AccountRoleEnum.Customer).ToListAsync();
            }
        }

        public async Task<bool> IsAccountExistWithEmailAsync(string email)
        {
            using (var context = new SmrmsContext())
            {
                return await context.Accounts.AnyAsync(a => a.Email.Equals(email));
            }
        }

        public async Task<Account> GetAccountAsyncById(int accountId)
        {
            using (var context = new SmrmsContext())
            {
                return await context.Accounts
                    .Include(a => a.AccountBusinesses)
                    .FirstOrDefaultAsync(a => a.AccountId == accountId);
            }
        }

        public async Task<bool> IsAccountExistWithUsernameAsync(string username)
        {
            using (var context = new SmrmsContext())
            {
                return await context.Accounts.AnyAsync(a => a.Username.Equals(username));
            }
        }
    }
}
