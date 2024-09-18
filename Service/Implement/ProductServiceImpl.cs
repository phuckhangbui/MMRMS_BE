using DTOs.Product;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductServiceImpl(IProductRepository productRepository, IComponentRepository componentRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _componentRepository = componentRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetProductList()
        {
            var list = await _productRepository.GetProductList();

            if (list.IsNullOrEmpty())
            {
                throw new ServiceException("There is no product available");
            }

            return list;
        }

        public async Task<DisplayProductDetailDto> GetProductDetailDto(int productId)
        {
            var productDetail = await _productRepository.GetProductDetail(productId);

            if (productDetail == null)
            {
                throw new ServiceException("There is no product with the id: " + productId);
            }

            return productDetail;
        }

        public async Task<IEnumerable<SerialProductNumberDto>> GetSerialProductList(int productId)
        {
            var isProductExisted = await _productRepository.IsProductExisted(productId);

            if (isProductExisted)
            {
                var list = await _productRepository.GetProductNumberList(productId);
                if (list.IsNullOrEmpty())
                {
                    throw new ServiceException("There is no available serial product number with the id: " + productId);
                }

                return list;
            }

            throw new ServiceException("There is no product with the id: " + productId);

        }

        public async Task<ProductDto> CreateProduct(CreateProductDto createProductDto)
        {
            if (createProductDto == null)
            {
                throw new ServiceException("There is no product to create");
            }

            var category = await _categoryRepository.GetCategoryById(createProductDto.CategoryId);

            if (category == null)
            {
                throw new ServiceException("There is no category with id: " + createProductDto.CategoryId);

            }

            var flag = true;


            if (!createProductDto.ExistedComponentList.IsNullOrEmpty())
            {
                foreach (var component in createProductDto.ExistedComponentList)
                {
                    if (!await _componentRepository.IsComponentIdExisted(component.ComponentId))
                    {
                        flag = false;
                    }
                }
            }

            if (!flag)
            {
                throw new ServiceException("The component id list provided is not correct");
            }

            if (!createProductDto.NewComponentList.IsNullOrEmpty())
            {
                foreach (var component in createProductDto.NewComponentList)
                {
                    if (await _componentRepository.IsComponentNameExisted(component.ComponentName))
                    {
                        flag = false;
                    }
                }
            }

            if (!flag)
            {
                throw new ServiceException("The newly added component name already exist in database, please provide a id or a new name");
            }

            var productDto = await _productRepository.CreateProduct(createProductDto);

            return productDto;
        }
    }
}
