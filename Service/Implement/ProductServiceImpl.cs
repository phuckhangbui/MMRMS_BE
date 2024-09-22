using AutoMapper;
using Common;
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
        private readonly IMapper _mapper;

        public ProductServiceImpl(IProductRepository productRepository, IComponentRepository componentRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _componentRepository = componentRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProductList()
        {
            var list = await _productRepository.GetProductList();

            if (list.IsNullOrEmpty())
            {
                return null;
            }

            return list;
        }

        public async Task<DisplayProductDetailDto> GetProductDetailDto(int productId)
        {
            var productDetail = await _productRepository.GetProductDetail(productId);

            if (productDetail == null)
            {
                throw new ServiceException(MessageConstant.Product.ProductNotFound);
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
                    return null;
                }


                return _mapper.Map<IEnumerable<SerialProductNumberDto>>(list);
            }

            throw new ServiceException(MessageConstant.Product.ProductNotFound);

        }

        public async Task<ProductDto> CreateProduct(CreateProductDto createProductDto)
        {
            if (await _productRepository.IsProductExisted(createProductDto.ProductName))
            {
                throw new ServiceException(MessageConstant.Product.ProductNameDuplicated);
            }

            var category = await _categoryRepository.GetCategoryById(createProductDto.CategoryId);

            if (category == null)
            {
                throw new ServiceException(MessageConstant.Category.CategoryNotFound);

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
                throw new ServiceException(MessageConstant.Product.ComponentIdListNotCorrect);
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
                throw new ServiceException(MessageConstant.Component.ComponetNameDuplicated);
            }

            var productDto = await _productRepository.CreateProduct(createProductDto);

            return productDto;
        }
    }
}
