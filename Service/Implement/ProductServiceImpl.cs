using AutoMapper;
using Common;
using Common.Enum;
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

        public async Task<IEnumerable<SerialNumberProductDto>> GetSerialProductList(int productId)
        {
            var isProductExisted = await _productRepository.IsProductExisted(productId);

            if (isProductExisted)
            {
                var list = await _productRepository.GetProductNumberList(productId);
                return _mapper.Map<IEnumerable<SerialNumberProductDto>>(list);
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

            if (createProductDto.ImageUrls.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Product.ImageIsRequired);
            }

            if (createProductDto.ImageUrls.Count() < 1)
            {
                throw new ServiceException(MessageConstant.Product.ImageIsRequired);
            }

            var productDto = await _productRepository.CreateProduct(createProductDto);

            return productDto;
        }

        public async Task DeleteProduct(int productId)
        {
            var productDto = await _productRepository.GetProduct(productId);

            var productNumberList = await _productRepository.GetProductNumberList(productId);

            if (productDto == null)
                throw new ServiceException(MessageConstant.Product.ProductNotFound);

            if (!productNumberList.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Product.ProductHasSerialNumberCannotDeleted);

            }

            await _productRepository.DeleteProduct(productId);
            //else productDto.IsDelete = true;

            //await _productRepository.UpdateProduct(productDto);
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

        public async Task UpdateProductAttribute(int productId, IEnumerable<CreateProductAttributeDto> productAttributeDtos)
        {
            var product = await _productRepository.GetProduct(productId);

            if (product == null)
            {
                throw new ServiceException(MessageConstant.Product.ProductNotFound);
            }

            await _productRepository.UpdateProductAttribute(productId, productAttributeDtos);
        }

        public async Task UpdateProductComponent(int productId, ComponentList componentList)
        {
            var product = await _productRepository.GetProduct(productId);

            if (product == null)
            {
                throw new ServiceException(MessageConstant.Product.ProductNotFound);
            }

            var serialProducts = await _productRepository.GetProductNumberList(productId);

            if (!serialProducts.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Product.ProductHasSerialNumberCannotUpdateComponentList);
            }

            if (!componentList.ExistedComponentList.IsNullOrEmpty())
                foreach (var component in componentList.ExistedComponentList)
                {
                    if (!await _componentRepository.IsComponentIdExisted(component.ComponentId))
                    {
                        throw new ServiceException(MessageConstant.Component.ComponentNotExisted);
                    }
                }
            if (!componentList.NewComponentList.IsNullOrEmpty())
                foreach (var component in componentList.NewComponentList)
                {
                    if (await _componentRepository.IsComponentNameExisted(component.ComponentName))
                    {
                        throw new ServiceException(MessageConstant.Component.ComponetNameDuplicated);
                    }
                }

            await _productRepository.UpdateProductComponent(productId, componentList);
        }


        public async Task ChangeProductImages(int productId, List<ImageList> imageList)
        {
            var product = await _productRepository.GetProduct(productId);

            if (product == null)
            {
                throw new ServiceException(MessageConstant.Product.ProductNotFound);
            }


            if (imageList.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Product.ImageIsRequired);
            }

            await _productRepository.UpdateProductImage(productId, imageList);

        }

        public async Task UpdateProductTerm(int productId, IEnumerable<CreateProductTermDto> productTermDtos)
        {
            var product = await _productRepository.GetProduct(productId);

            if (product == null)
            {
                throw new ServiceException(MessageConstant.Product.ProductNotFound);
            }

            await _productRepository.UpdateProductTerm(productId, productTermDtos);
        }

        public async Task<IEnumerable<ProductDto>> GetTop8LatestProductList()
        {
            return await _productRepository.GetTop8LatestProductList();
        }

        public async Task<IEnumerable<ProductReviewDto>> GetProductReviews(List<int> productIds)
        {
            return await _productRepository.GetProductReviews(productIds);
        }

        public async Task ToggleLockStatus(int productId)
        {

            var productDto = await _productRepository.GetProduct(productId);

            if (productDto == null)
            {
                throw new ServiceException(MessageConstant.Product.ProductNotFound);
            }

            //if (productDto.Status == ProductStatusEnum.NoSerialMachine.ToString())
            //{
            //    throw new ServiceException(MessageConstant.Product.ProductStateNotSuitableForModifyStatus);
            //}

            if (productDto.Status != ProductStatusEnum.Locked.ToString())
            {
                productDto.Status = ProductStatusEnum.Locked.ToString();

                await _productRepository.UpdateProduct(productDto);

                return;
            }

            // when status is currently "locked"
            var serialProductList = await GetSerialProductList(productId);

            if (serialProductList.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Product.ProductStateNotSuitableForModifyStatus);
            }

            foreach (var serialProduct in serialProductList)
            {
                if (serialProduct.Status == SerialNumberProductStatusEnum.Available.ToString())
                {
                    productDto.Status = ProductStatusEnum.Active.ToString();

                    await _productRepository.UpdateProduct(productDto);

                    return;
                }
            }

            productDto.Status = ProductStatusEnum.OutOfStock.ToString();

            await _productRepository.UpdateProduct(productDto);
        }
    }
}
