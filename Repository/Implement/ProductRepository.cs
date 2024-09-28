using AutoMapper;
using BusinessObject;
using DAO;
using DAO.Enum;
using DTOs.Product;
using DTOs.SerialNumberProduct;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class ProductRepository : IProductRepository
    {

        private readonly IMapper _mapper;

        public ProductRepository(IMapper mapper)
        {
            _mapper = mapper;
        }


        public async Task<IEnumerable<ProductDto>> GetProductList()
        {
            var productList = await ProductDao.Instance.GetProductListWithCategory();

            if (!productList.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<ProductDto>>(productList);
            }

            return null;
        }

        public async Task<ProductDto> GetProduct(int productId)
        {
            var product = await ProductDao.Instance.GetProduct(productId);

            if (product == null)
                return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<DisplayProductDetailDto> GetProductDetail(int productId)
        {
            var product = await ProductDao.Instance.GetProductDetail(productId);

            if (product == null)
            {
                return null;
            }

            var productDetail = _mapper.Map<DisplayProductDetailDto>(product);

            return productDetail;
        }

        public async Task<IEnumerable<SerialProductNumberDto>> GetProductNumberList(int productId)
        {
            var product = await ProductDao.Instance.GetProductWithSerialProductNumber(productId);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<SerialProductNumberDto>>(product.SerialNumberProducts);
        }

        public async Task<ProductDto> CreateProduct(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);

            if (product == null)
            {
                return null;
            }

            var componentProducts = new List<ComponentProduct>();

            if (!createProductDto.ExistedComponentList.IsNullOrEmpty())
            {
                foreach (AddExistedComponentToProduct component in createProductDto.ExistedComponentList)
                {
                    var componentProduct = _mapper.Map<ComponentProduct>(component);
                    componentProduct.Status = ProductComponentStatusEnum.Normal.ToString();
                    componentProducts.Add(componentProduct);
                }
            }

            product.ComponentProducts = componentProducts;

            product.Quantity = 0;
            product.DateCreate = DateTime.Now;
            product.Status = ProductStatusEnum.NoSerialMachine.ToString();

            product = await ProductDao.Instance.CreateProduct(product, createProductDto.NewComponentList);


            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> IsProductExisted(int productId)
        {
            return await ProductDao.Instance.IsProductExisted(productId);
        }

        public async Task<bool> IsProductExisted(string name)
        {
            return await ProductDao.Instance.IsProductExisted(name);
        }

        public async Task<bool> IsProductModelExisted(string model)
        {
            return await ProductDao.Instance.IsProductModelExisted(model);
        }

        public async Task UpdateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            await ProductDao.Instance.UpdateAsync(product);
        }

        public async Task DeleteProduct(int productId)
        {
            await ProductDao.Instance.DeleteProduct(productId);
        }

        public async Task UpdateProductAttribute(int productId, IEnumerable<CreateProductAttributeDto> productAttributeDtos)
        {
            var product = await ProductDao.Instance.GetProductDetail(productId);

            product.ProductAttributes = new List<ProductAttribute>();

            foreach (var attributeDto in productAttributeDtos)
            {
                var attribute = new ProductAttribute
                {
                    ProductId = product.ProductId,
                    AttributeName = attributeDto.AttributeName,
                    Unit = attributeDto.Unit,
                    Specifications = attributeDto.Specifications
                };

                product.ProductAttributes.Add(attribute);
            }

            await ProductDao.Instance.UpdateAsync(product);
        }

        public async Task UpdateProductComponent(int productId, ComponentList componentList)
        {
            var product = await ProductDao.Instance.GetProductDetail(productId);

            var componentProducts = new List<ComponentProduct>();

            if (!componentList.ExistedComponentList.IsNullOrEmpty())
            {
                foreach (AddExistedComponentToProduct component in componentList.ExistedComponentList)
                {
                    var componentProduct = _mapper.Map<ComponentProduct>(component);
                    componentProduct.Status = ProductComponentStatusEnum.Normal.ToString();
                    componentProducts.Add(componentProduct);
                }
            }

            product.ComponentProducts = componentProducts;

            await ProductDao.Instance.UpdateProductComponent(product, componentList.NewComponentList);
        }
    }
}
