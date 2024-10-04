using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class AccountLogDao : BaseDao<Log>
    {
        private static AccountLogDao instance = null;
        private static readonly object instacelock = new object();

        private AccountLogDao()
        {

        }

        public static AccountLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountLogDao();
                }
                return instance;
            }
        }

        public async Task<Log> GetAccountLogByAccountId(int accountId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Logs
                    .FirstOrDefaultAsync(ap => ap.AccountLogId == accountId);
            }
        }
    }
}
