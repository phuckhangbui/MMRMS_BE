using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class MembershipRankLogDao : BaseDao<MembershipRankLog>
    {
        private static MembershipRankLogDao instance = null;
        private static readonly object instacelock = new object();

        private MembershipRankLogDao()
        {

        }

        public static MembershipRankLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MembershipRankLogDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<MembershipRankLog>> GetMembershipRankLogsByAccountId(int accountId)
        {
            using var context = new MmrmsContext();

            return await context.MembershipRankLogs
                .Where(x => x.AccountId == accountId)
                .ToListAsync();
        }
    }
}
