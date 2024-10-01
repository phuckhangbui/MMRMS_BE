using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class AccountBusinessDao : BaseDao<AccountBusiness>
    {
        private static AccountBusinessDao instance = null;
        private static readonly object instacelock = new object();

        private AccountBusinessDao()
        {

        }

        public static AccountBusinessDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountBusinessDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<AccountBusiness>> GetAccountBusinessesByAccountId(int accountId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.AccountBusinesses
                    .Where(a => a.AccountId == accountId)
                    .ToListAsync();
            }
        }
    }
}
