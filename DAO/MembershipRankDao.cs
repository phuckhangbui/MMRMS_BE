using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class MembershipRankDao : BaseDao<MembershipRank>
    {
        private static MembershipRankDao instance = null;
        private static readonly object instacelock = new object();

        private MembershipRankDao()
        {

        }

        public static MembershipRankDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MembershipRankDao();
                }
                return instance;
            }
        }

        public async Task<MembershipRank> GetMembershipRankById(int membershipRankId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MembershipRanks.FirstOrDefaultAsync(m => m.MembershipRankId == membershipRankId);
            }
        }

        public async Task<MembershipRank?> GetMembershipRanksForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                var account = await context.Accounts
                    .Include(a => a.MembershipRank)
                    .FirstOrDefaultAsync(a => a.AccountId == customerId);

                return account?.MembershipRank;
            }
        }
    }
}
