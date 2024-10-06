using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class AccountLogDetailDao : BaseDao<LogDetail>
    {
        private static AccountLogDetailDao instance = null;
        private static readonly object instacelock = new object();

        private AccountLogDetailDao()
        {

        }

        public static AccountLogDetailDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountLogDetailDao();
                }
                return instance;
            }
        }


        public async Task<IEnumerable<LogDetail>> GetLogDetails(int accountId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.LogDetails
                    .Where(l => l.AccountId == accountId)
                    .ToListAsync();
            }
        }

    }
}
