using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ProductDao : BaseDao<Product>
    {
        private static ProductDao instance = null;
        private static readonly object instacelock = new object();

        private ProductDao()
        {

        }

        public static ProductDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductDao();
                }
                return instance;
            }
        }

        public async Task<bool> IsProductExisted(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products.AnyAsync(p => p.ProductId == productId);

            }
        }

        public async Task<IEnumerable<Product>> GetProductListWithCategory()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products.Include(p => p.Category).Include(p => p.ProductImages).ToListAsync();
            }
        }

        public async Task<Product> GetProductDetail(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products.Include(p => p.Category)
                                             .Include(p => p.ProductImages)
                                             .Include(p => p.ProductAttributes)
                                             .Include(p => p.ComponentProducts)
                                             .FirstOrDefaultAsync(p => p.ProductId == productId);
            }
        }

        public async Task<Product> GetProductWithSerialProductNumber(int productId)
        {
            using (var context = new MmrmsContext())
            {
                var product = await context.Products.Include(p => p.SerialNumberProducts).FirstOrDefaultAsync(p => p.ProductId == productId);

                return product;

            }

        }

        public async Task CreateProduct(Product product)
        {
            using (var context = new MmrmsContext())
            {
                DbSet<Product> _dbSet = context.Set<Product>();
                _dbSet.Add(product);
                await context.SaveChangesAsync();
            }
        }
    }
}
