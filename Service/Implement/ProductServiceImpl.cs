using AutoMapper;
using Common;
using DAO.Enum;
using DTOs.Product;
using DTOs.SerialNumberProduct;
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

            if (await _productRepository.IsProductModelExisted(createProductDto.Model))
            {
                throw new ServiceException(MessageConstant.Product.ProductModelDuplicated);
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

        public async Task ToggleProductIsDelete(int productId)
        {
            var productDto = await _productRepository.GetProduct(productId);

            if (productDto == null)
                throw new ServiceException(MessageConstant.Product.ProductNotFound);

            if ((bool)productDto.IsDelete)
            {
                productDto.IsDelete = false;
            }
            else productDto.IsDelete = true;

            await _productRepository.UpdateProduct(productDto);
        }

        public async Task UpdateProductStatus(int productId, string status)
        {
            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(ProductStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.Product.StatusNotAvailable);
            }

            var productDto = await _productRepository.GetProduct(productId);

            if (productDto == null)
                throw new ServiceException(MessageConstant.Product.ProductNotFound);

            productDto.Status = status;

            await _productRepository.UpdateProduct(productDto);
        }

        public async Task UpdateProductDetail(int productId, UpdateProductDto updateProductDto)
        {
            var productDto = await _productRepository.GetProduct(productId);
            if (productDto == null)
            {
                throw new ServiceException(MessageConstant.Product.ProductNotFound);
            }

            if (!productDto.ProductName.ToLower().Equals(updateProductDto.ProductName.ToLower()))
            {
                if (await _productRepository.IsProductExisted(updateProductDto.ProductName))
                {
                    throw new ServiceException(MessageConstant.Product.ProductNameDuplicated);
                }
            }
            if (!productDto.Model.ToLower().Equals(updateProductDto.Model.ToLower()))
            {
                if (await _productRepository.IsProductModelExisted(updateProductDto.Model))
                {
                    throw new ServiceException(MessageConstant.Product.ProductModelDuplicated);
                }
            }

            var category = await _categoryRepository.GetCategoryById(updateProductDto.CategoryId);

            if (category == null)
            {
                throw new ServiceException(MessageConstant.Category.CategoryNotFound);

            }

            productDto.ProductName = updateProductDto.ProductName;
            productDto.Description = updateProductDto.Description;
            productDto.RentPrice = updateProductDto.RentPrice;
            productDto.ProductPrice = updateProductDto.ProductPrice;
            productDto.Model = updateProductDto.Model;
            productDto.Origin = updateProductDto.Origin;
            productDto.CategoryId = updateProductDto.CategoryId;

            await _productRepository.UpdateProduct(productDto);
        }
    }
}
