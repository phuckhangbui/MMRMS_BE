using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class RentingServiceDao : BaseDao<RentingService>
    {
        private static RentingServiceDao instance = null;
        private static readonly object instacelock = new object();

        private RentingServiceDao()
        {

        }

        public static RentingServiceDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RentingServiceDao();
                }
                return instance;
            }
        }

        public async Task<RentingService> GetRentingServiceById(int rentingServiceId)
        {
            using var context = new MmrmsContext();
            return await context.RentingServices.FirstOrDefaultAsync(rs => rs.RentingServiceId == rentingServiceId);
        }
    }
}
