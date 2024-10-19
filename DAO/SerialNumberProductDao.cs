using Common.Enum;
using Microsoft.EntityFrameworkCore;
using SerialNumberProduct = BusinessObject.SerialNumberProduct;

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
                            && s.Status == SerialNumberProductStatusEnum.Available.ToString());
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
                    .OrderByDescending(s => s.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<bool> IsSerialNumberInAnyContract(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .AnyAsync(s => s.SerialNumber.Equals(serialNumber));
            }
        }

        public async Task Delete(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                var serialNumberProduct = await context.SerialNumberProducts
                    .FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                if (serialNumberProduct != null)
                {
                    DbSet<SerialNumberProduct> _dbSet = context.Set<SerialNumberProduct>();
                    _dbSet.Remove(serialNumberProduct);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateStatus(string serialNumber, string status)
        {
            using (var context = new MmrmsContext())
            {
                var serialNumberProduct = await context.SerialNumberProducts
                    .FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                if (serialNumberProduct != null)
                {
                    serialNumberProduct.Status = status;
                    DbSet<SerialNumberProduct> _dbSet = context.Set<SerialNumberProduct>();
                    var tracker = context.Attach(serialNumberProduct);
                    tracker.State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
        }


        public async Task<SerialNumberProduct> GetSerialNumberProduct(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                var serialNumberProduct = await context.SerialNumberProducts
                    .FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                return serialNumberProduct;
            }
        }

        public async Task<SerialNumberProduct> GetSerialNumberProductDetail(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                var serialNumberProduct = await context.SerialNumberProducts.Include(c => c.SerialNumberProductLogs).Include(c => c.ProductComponentStatuses).FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                return serialNumberProduct;
            }
        }

        //public async Task CreateSerialProduct(SerialNumberProduct serialProduct)
        //{
        //    using (var context = new MmrmsContext())
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                DbSet<SerialNumberProduct> _dbSet = context.Set<SerialNumberProduct>();
        //                _dbSet.Add(serialProduct);
        //                await context.SaveChangesAsync();

        //                var serialNumberProduct

        //                await transaction.CommitAsync();
        //            }
        //            catch (Exception e)
        //            {
        //                transaction.Rollback();
        //                throw new Exception(e.Message);
        //            }
        //        }
        //    }
        //}

        public async Task<bool> IsSerialNumberProductValidToRent(int productId, int quantity, DateTime startDate, int numberOfMonth)
        {
            using var context = new MmrmsContext();

            var requestedEndDate = startDate.AddMonths(numberOfMonth);

            var availableSerialNumbers = await context.SerialNumberProducts
                .Where(s => s.ProductId == productId
                        && s.Status == SerialNumberProductStatusEnum.Available.ToString())
                .Select(s => s.SerialNumber)
                .ToListAsync();

            var serialNumbersInFutureContracts = await context.Contracts
                .Where(c => availableSerialNumbers.Contains(c.SerialNumber!)
                        && (c.Status == ContractStatusEnum.NotSigned.ToString() || c.Status == ContractStatusEnum.Signed.ToString() || c.Status == ContractStatusEnum.Shipping.ToString() || c.Status == ContractStatusEnum.Shipped.ToString())
                        && (c.DateStart < requestedEndDate && c.DateEnd > startDate))
                .Select(c => c.SerialNumber)
                .ToListAsync();

            var suitableAvailableSerialNumbers = availableSerialNumbers
                .Except(serialNumbersInFutureContracts)
                .ToList();

            return suitableAvailableSerialNumbers.Count >= quantity;
        }

        public async Task<List<SerialNumberProduct>> GetSerialNumberProductValidToRent(int productId, DateTime startDate, int numberOfMonth)
        {
            using var context = new MmrmsContext();

            var requestedEndDate = startDate.AddMonths(numberOfMonth);

            var availableSerialNumbers = await context.SerialNumberProducts
                .Where(s => s.ProductId == productId
                        && s.Status == SerialNumberProductStatusEnum.Available.ToString())
                .ToListAsync();

            var serialNumbersInFutureContracts = await context.Contracts
                .Where(c => availableSerialNumbers.Contains(c.ContractSerialNumberProduct)
                        && (c.Status == ContractStatusEnum.Signed.ToString() || c.Status == ContractStatusEnum.Shipping.ToString() || c.Status == ContractStatusEnum.Shipped.ToString())
                        && (c.DateStart < requestedEndDate && c.DateEnd > startDate))
                .Select(c => c.ContractSerialNumberProduct)
                .ToListAsync();

            var suitableAvailableSerialNumbers = availableSerialNumbers
                .Except(serialNumbersInFutureContracts)
                .ToList();

            return suitableAvailableSerialNumbers;
        }
    }
}
