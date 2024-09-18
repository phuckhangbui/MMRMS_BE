using BusinessObject;
using DAO.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class PromotionDao : BaseDao<Promotion>
    {
        private static PromotionDao instance = null;
        private static readonly object instacelock = new object();

        private PromotionDao()
        {

        }

        public static PromotionDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PromotionDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsForUpdateToExpired()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Promotions.Where(p => p.DateEnd == DateTime.Now.Date.AddDays(-1)).ToListAsync();
            }
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsForUpdateToActive()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Promotions.Where(p => p.DateStart == DateTime.Now.Date).ToListAsync();
            }
        }
    }
}
