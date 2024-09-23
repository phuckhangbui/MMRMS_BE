using BusinessObject;
using DAO.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ProductNumberDao : BaseDao<SerialNumberProduct>
    {
        private static ProductNumberDao instance = null;
        private static readonly object instacelock = new object();

        private ProductNumberDao()
        {

        }

        public static ProductNumberDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductNumberDao();
                }
                return instance;
            }
        }

        public async Task<SerialNumberProduct> GetSerialNumberProductBySerialNumberAndProductId(string serialNumber, int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.ProductNumbers
                    .FirstOrDefaultAsync(s => s.ProductId == productId && s.SerialNumber.Equals(serialNumber));
            }
        }

        public async Task<bool> IsSerialNumberProductValidToRent(string serialNumber, int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.ProductNumbers
                    .AnyAsync(s => s.ProductId == productId 
                            && s.SerialNumber.Equals(serialNumber)
                            && s.IsDelete == false
                            && s.Status == SerialMachineStatusEnum.Available.ToString());
            }
        }
    }
}
