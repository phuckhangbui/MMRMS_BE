using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class DeliveryDao : BaseDao<Delivery>
    {
        private static DeliveryDao instance = null;
        private static readonly object instacelock = new object();

        private DeliveryDao()
        {

        }

        public static DeliveryDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeliveryDao();
                }
                return instance;
            }
        }

        //TODO:KHANG
        public async Task<IEnumerable<Delivery>> GetDeliveries()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.Contract)
                    //.ThenInclude(c => c.ContractAddress)
                    .OrderByDescending(p => p.DateCreate).ToListAsync();
            }
        }

        //TODO:KHANG
        public async Task<IEnumerable<Delivery>> GetDeliveriesForStaff(int staffId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.Contract)
                    //.ThenInclude(c => c.ContractAddress)
                    .OrderByDescending(p => p.DateCreate)
                    .Where(d => d.StaffId == staffId)
                    .ToListAsync();
            }
        }

        //TODO:KHANG
        public async Task<Delivery> GetDelivery(int deliveryId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Deliveries
                    .Include(d => d.Staff)
                    .Include(d => d.Contract)
                    //.ThenInclude(c => c.ContractAddress)
                    .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
            }
        }
    }
}
