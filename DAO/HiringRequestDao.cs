using BusinessObject;
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
                    .Include(h => h.RentingRequestProductDetails)
                    .FirstOrDefaultAsync(h => h.RentingRequestId.Equals(rentingRequestId));
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
    }
}
