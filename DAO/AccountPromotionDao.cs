using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class AccountPromotionDao : BaseDao<AccountPromotion>
    {
        private static AccountPromotionDao instance = null;
        private static readonly object instacelock = new object();

        private AccountPromotionDao()
        {

        }

        public static AccountPromotionDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountPromotionDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<AccountPromotion>> GetPromotionsByCustomerId(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.AccountPromotions
                    .Where(ap => ap.AccountId == customerId)
                        .Include(ap => ap.Promotion)
                    .ToListAsync();
            }
        }
    }
}
