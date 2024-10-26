using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class DeliveryTaskDao : BaseDao<DeliveryTask>
    {
        private static DeliveryTaskDao instance = null;
        private static readonly object instacelock = new object();

        private DeliveryTaskDao()
        {

        }

        public static DeliveryTaskDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeliveryTaskDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<DeliveryTask>> GetDeliveries()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .OrderByDescending(p => p.DateCreate).ToListAsync();
            }
        }

        public async Task<IEnumerable<DeliveryTask>> GetDeliveriesForStaff(int staffId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .OrderByDescending(p => p.DateCreate)
                    .Where(d => d.StaffId == staffId)
                    .ToListAsync();
            }
        }

        public async Task<DeliveryTask> GetDeliveryTask(int DeliveryTaskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.ContractDeliveries)
                    .ThenInclude(d => d.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .FirstOrDefaultAsync(d => d.DeliveryTaskId == DeliveryTaskId);
            }
        }
    }
}
