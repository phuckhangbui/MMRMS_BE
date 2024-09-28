using BusinessObject;
using DAO.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class SerialNumberProductDao : BaseDao<SerialNumberProduct>
    {
        private static SerialNumberProductDao instance = null;
        private static readonly object instacelock = new object();

        private SerialNumberProductDao()
        {

        }

        public static SerialNumberProductDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SerialNumberProductDao();
                }
                return instance;
            }
        }

        public async Task<SerialNumberProduct> GetSerialNumberProductBySerialNumberAndProductId(string serialNumber, int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.SerialNumberProducts
                    .FirstOrDefaultAsync(s => s.ProductId == productId && s.SerialNumber.Equals(serialNumber));
            }
        }

        public async Task<bool> IsSerialNumberProductValidToRent(string serialNumber, int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.SerialNumberProducts
                    .AnyAsync(s => s.ProductId == productId
                            && s.SerialNumber.Equals(serialNumber)
                            && s.Status == SerialNumberProductStatus.Available.ToString());
            }
        }

        public async Task<bool> IsSerialNumberExisted(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                return await context.SerialNumberProducts
                    .AnyAsync(s => s.SerialNumber.Equals(serialNumber));
            }
        }

        public async Task<IEnumerable<SerialNumberProduct>> GetSerialNumberProductsByProductIdAndStatus(int productId, string status)
        {
            using (var context = new MmrmsContext())
            {
                return await context.SerialNumberProducts
                    .Where(s => s.ProductId == productId && s.Status.Equals(status))
                    .ToListAsync();
            }
        }
    }
}
