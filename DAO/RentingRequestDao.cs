using BusinessObject;
using Common.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class RentingRequestDao : BaseDao<RentingRequest>
    {
        private static RentingRequestDao instance = null;
        private static readonly object instacelock = new object();

        private RentingRequestDao()
        {

        }

        public static RentingRequestDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RentingRequestDao();
                }
                return instance;
            }
        }

        public async Task<RentingRequest> GetRentingRequestById(string rentingRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .Include(rr => rr.RentingRequestProductDetails)
                        .ThenInclude(rr => rr.Product)
                    .Include(rr => rr.ServiceRentingRequests)
                        .ThenInclude(rr => rr.RentingService)
                    .Include(rr => rr.AccountOrder)
                        .ThenInclude(rr => rr.Addresses)
                    .FirstOrDefaultAsync(rr => rr.RentingRequestId.Equals(rentingRequestId));
            }
        }

        public async Task<IEnumerable<RentingRequest>> GetRentingRequests()
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests.Include(h => h.AccountOrder).ToListAsync();
            }
        }

        public async Task<RentingRequest> GetRentingRequestByIdAndStatus(string rentingRequestId, string status)
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .FirstOrDefaultAsync(h => h.RentingRequestId.Equals(rentingRequestId)
                        && h.Status.Equals(status));
            }
        }

        public async Task<IEnumerable<RentingRequest>> GetRentingRequestsForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .Where(rr => rr.AccountOrderId == customerId)
                    .Include(h => h.AccountOrder)
                    .ToListAsync();
            }
        }

        public async Task<RentingRequest> CreateRentingRequest(RentingRequest rentingRequest, int accountPromotionId)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (accountPromotionId != 0)
                        {
                            var accountPromotion = await context.AccountPromotions
                                .FirstOrDefaultAsync(ap => ap.AccountPromotionId == accountPromotionId);

                            if (accountPromotion != null && accountPromotion.Status!.Equals(AccountPromotionStatusEnum.Active.ToString()))
                            {
                                accountPromotion.Status = AccountPromotionStatusEnum.Redeemed.ToString();
                                context.AccountPromotions.Update(accountPromotion);
                            }
                        }

                        DbSet<RentingRequest> _dbSet = context.Set<RentingRequest>();
                        _dbSet.Add(rentingRequest);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return rentingRequest;
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
