using BusinessObject;
using Common.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Component = BusinessObject.Component;
using Product = BusinessObject.Product;
using ProductAttribute = BusinessObject.ProductAttribute;

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
                return await context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductTerms)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

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
                var list = await context.Products.Include(p => p.Category).Include(p => p.ProductImages).ToListAsync();

                return list;
            }
        }

        public async Task<Product> GetProductDetail(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products.Include(p => p.Category)
                                             .Include(p => p.ProductImages)
                                             .Include(p => p.ProductAttributes)
                                             .Include(p => p.ProductTerms)
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

        public async Task DeleteProduct(int productId)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var product = await context.Products.Include(p => p.Category)
                                            .Include(p => p.ProductImages)
                                            .Include(p => p.ProductAttributes)
                                            .Include(p => p.ComponentProducts)
                                            .ThenInclude(c => c.Component)
                                            .FirstOrDefaultAsync(p => p.ProductId == productId);

                        foreach (var image in product.ProductImages)
                        {
                            DbSet<ProductImage> _dbSet = context.Set<ProductImage>();
                            _dbSet.Remove(image);

                        }

                        foreach (var attribute in product.ProductAttributes)
                        {
                            DbSet<ProductAttribute> _dbSet = context.Set<ProductAttribute>();
                            _dbSet.Remove(attribute);

                        }

                        foreach (var componentProduct in product.ComponentProducts)
                        {
                            DbSet<ComponentProduct> _dbSet = context.Set<ComponentProduct>();
                            _dbSet.Remove(componentProduct);

                        }

                        product.ComponentProducts = null;
                        product.ProductImages = null;
                        product.ProductAttributes = null;

                        DbSet<Product> dbSet = context.Set<Product>();
                        dbSet.Remove(product);

                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }

            }
        }

        public async Task<Product> CreateProduct(Product product, List<Tuple<Component, int, bool>>? newComponentList)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!newComponentList.IsNullOrEmpty())
                        {
                            foreach (var tuple in newComponentList)
                            {
                                var component = tuple.Item1;
                                var quantity = tuple.Item2;
                                var isRequireMoney = tuple.Item3;

                                context.Components.Add(component);
                                await context.SaveChangesAsync();

                                var componentProduct = new ComponentProduct()
                                {
                                    ComponentId = component.ComponentId,
                                    ProductId = product.ProductId,
                                    Quantity = quantity,
                                    Status = ProductComponentStatusEnum.Normal.ToString(),
                                    IsRequiredMoney = isRequireMoney,
                                };

                                // Add the ComponentProduct to the product's ComponentProducts
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

        public async Task UpdateProductComponent(Product product, List<Tuple<Component, int, bool>>? newComponentList, IEnumerable<ComponentProduct>? componentProducts)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var oldComponentProducts = context.ComponentProducts
                         .Where(cp => cp.ProductId == product.ProductId);

                        context.ComponentProducts.RemoveRange(oldComponentProducts);
                        await context.SaveChangesAsync();

                        //Handle the new component list
                        if (!newComponentList.IsNullOrEmpty())
                        {
                            foreach (var tuple in newComponentList)
                            {
                                var component = tuple.Item1;
                                var quantity = tuple.Item2;
                                var isRequiredMoney = tuple.Item3;

                                context.Components.Add(component);
                                await context.SaveChangesAsync();

                                var componentProduct = new ComponentProduct()
                                {
                                    ComponentId = component.ComponentId,
                                    ProductId = product.ProductId,
                                    Quantity = quantity,
                                    Status = ProductComponentStatusEnum.Normal.ToString(),
                                    IsRequiredMoney = isRequiredMoney
                                };

                                // Add the ComponentProduct to the product's ComponentProducts
                                product.ComponentProducts.Add(componentProduct);
                            }
                        }

                        // Append the additional ComponentProducts to the product (if any)
                        var componentProductList = new List<ComponentProduct>();
                        if (!componentProducts.IsNullOrEmpty())
                        {
                            foreach (var cp in componentProducts)
                            {
                                cp.ProductId = product.ProductId;
                                componentProductList.Add(cp);
                            }
                        }

                        context.ComponentProducts.AddRange(componentProductList);

                        context.Products.Update(product);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(e.Message);
                    }
                }
            }
        }



        public async Task UpdateProductAttribute(Product product, IEnumerable<ProductAttribute> productAttributeLists)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var oldAttributeProducts = context.ProductAttributes
                       .Where(cp => cp.ProductId == product.ProductId);

                        context.ProductAttributes.RemoveRange(oldAttributeProducts);
                        await context.SaveChangesAsync();

                        product.ProductAttributes = (ICollection<ProductAttribute>)productAttributeLists;


                        context.Products.Update(product);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public async Task ChangeProductThumbnail(ProductImage productImage)
        {
            using (var context = new MmrmsContext())
            {
                var oldProductThumbnail = await context.ProductImages.FirstOrDefaultAsync(i => i.ProductId == productImage.ProductId && (bool)i.IsThumbnail);

                if (oldProductThumbnail != null)
                {
                    context.ProductImages.Remove(oldProductThumbnail);
                    await context.SaveChangesAsync();
                }

                DbSet<ProductImage> _dbSet = context.Set<ProductImage>();
                _dbSet.Add(productImage);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddProductImages(int productId, List<ProductImage> newProductImages)
        {
            using (var context = new MmrmsContext())
            {
                var oldNonThumbnailImages = context.ProductImages
                    .Where(i => i.ProductId == productId);

                context.ProductImages.RemoveRange(oldNonThumbnailImages);
                await context.SaveChangesAsync();

                context.ProductImages.AddRange(newProductImages);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductTerm(Product product, List<ProductTerm> productTerms)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var oldTermProducts = context.ProductTerms
                       .Where(cp => cp.ProductId == product.ProductId);

                        context.ProductTerms.RemoveRange(oldTermProducts);
                        await context.SaveChangesAsync();

                        product.ProductTerms = productTerms;


                        context.Products.Update(product);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public async Task<IEnumerable<Product>> GetTop8LatestProducts()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products
                    .Where(p => p.Status!.Equals(ProductStatusEnum.Active.ToString()))
                    .Include(p => p.ProductImages)
                    .Include(p => p.Category)
                    .OrderByDescending(p => p.DateCreate)
                    .Take(8)
                    .ToListAsync();
            }
        }

        public async Task<Product?> GetProductWithSerialProductNumberAndProductImages(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.SerialNumberProducts)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
            }
        }
    }
}
