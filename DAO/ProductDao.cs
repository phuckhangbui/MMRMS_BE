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

        public async Task<IEnumerable<Product>> GetProductListWithCategory()
        {
            using (var context = new SmrmsContext())
            {
                return await context.Products.Include(p => p.Category).ToListAsync();
            }
        }

    }
}
