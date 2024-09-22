using BusinessObject;
using DAO.Enum;
using DTOs.Component;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<Product> GetProduct(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            }
        }


        public async Task<bool> IsProductExisted(string name)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products.AnyAsync(p => p.ProductName.ToLower().Equals(name.Trim().ToLower()));

            }
        }

        public async Task<bool> IsProductModelExisted(string model)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products.AnyAsync(p => p.Model.ToLower().Equals(model.Trim().ToLower()));

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
                                             .ThenInclude(c => c.Component)
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

        public async Task<Product> CreateProduct(Product product, IEnumerable<CreateComponentEmbeddedDto>? newComponentList)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!newComponentList.IsNullOrEmpty())
                        {
                            foreach (var c in newComponentList)
                            {
                                Component Component = new Component
                                {
                                    ComponentName = c.ComponentName.Trim(),
                                    Quantity = null,
                                    Price = null,
                                    Status = ComponentStatusEnum.NoPriceAndQuantity.ToString(),
                                    DateCreate = DateTime.Now,
                                };

                                context.Components.Add(Component);
                                await context.SaveChangesAsync();

                                var componentProduct = new ComponentProduct()
                                {
                                    ComponentId = Component.ComponentId,
                                    Quantity = c.Quantity,
                                    Status = ProductComponentStatusEnum.Normal.ToString(),
                                };

                                product.ComponentProducts.Add(componentProduct);
                            }
                        }


                        DbSet<Product> _dbSet = context.Set<Product>();
                        _dbSet.Add(product);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return product;
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
