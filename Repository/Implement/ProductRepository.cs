﻿using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
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
            var list = await ProductDao.Instance.GetProductListWithCategory();

            var resultList = new List<ProductDto>();

            if (list.IsNullOrEmpty())
            {
                return null;
            }

            foreach (var product in list)
            {
                var productDto = _mapper.Map<ProductDto>(product);

                productDto.Quantity = product.SerialNumberProducts?.Count;

                resultList.Add(productDto);
            }

            return resultList;
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

            //Quantity availble
            var serialNumberProducts = await SerialNumberProductDao.Instance.GetSerialNumberProductsByProductIdAndStatus(productId, SerialNumberProductStatusEnum.Available.ToString());
            productDetail.Quantity = serialNumberProducts.Count();

            var prices = serialNumberProducts
                .Select(s => s.ActualRentPrice ?? 0)
                .OrderBy(s => s)
                .ToList();
            productDetail.RentPrices = prices;

            return productDetail;
        }

        public async Task<IEnumerable<SerialNumberProductDto>> GetProductNumberList(int productId)
        {
            var product = await ProductDao.Instance.GetProductWithSerialProductNumber(productId);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<SerialNumberProductDto>>(product.SerialNumberProducts);
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

            product.DateCreate = DateTime.Now;
            product.Status = ProductStatusEnum.Locked.ToString();

            List<Tuple<Component, int, bool>> componentsTuple = new List<Tuple<Component, int, bool>>();

            if (!createProductDto.NewComponentList.IsNullOrEmpty())
            {
                foreach (var component in createProductDto.NewComponentList)
                {
                    Component Component = new Component
                    {
                        ComponentName = component.ComponentName.Trim(),
                        Quantity = null,
                        Price = component.Price,
                        Status = ComponentStatusEnum.NoQuantity.ToString(),
                        DateCreate = DateTime.Now,
                    };

                    componentsTuple.Add(new Tuple<Component, int, bool>(Component, component.Quantity, component.IsRequiredMoney));

                }
            }
            var productImages = new List<ProductImage>();

            if (!createProductDto.ImageUrls.IsNullOrEmpty())
            {
                bool isFirstImage = true;

                foreach (var imageUrl in createProductDto.ImageUrls)
                {
                    var productImage = new ProductImage
                    {
                        ProductImageUrl = imageUrl.Url,
                        IsThumbnail = isFirstImage
                    };

                    productImages.Add(productImage);
                    isFirstImage = false;
                }
            }

            product.ProductImages = productImages;

            product = await ProductDao.Instance.CreateProduct(product, componentsTuple);


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

            var productAttributes = new List<ProductAttribute>();

            foreach (var attributeDto in productAttributeDtos)
            {
                var attribute = new ProductAttribute
                {
                    ProductId = product.ProductId,
                    AttributeName = attributeDto.AttributeName,
                    Unit = attributeDto.Unit,
                    Specifications = attributeDto.Specifications
                };

                productAttributes.Add(attribute);
            }

            await ProductDao.Instance.UpdateProductAttribute(product, productAttributes);
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

            List<Tuple<Component, int, bool>> components = new List<Tuple<Component, int, bool>>();
            if (!componentList.NewComponentList.IsNullOrEmpty())
                foreach (var component in componentList.NewComponentList)
                {
                    Component Component = new Component
                    {
                        ComponentName = component.ComponentName.Trim(),
                        Quantity = null,
                        Price = component.Price,
                        Status = ComponentStatusEnum.NoQuantity.ToString(),
                        DateCreate = DateTime.Now,
                    };

                    components.Add(new Tuple<Component, int, bool>(Component, component.Quantity, component.IsRequiredMoney));
                }

            await ProductDao.Instance.UpdateProductComponent(product, components, componentProducts);
        }

        public async Task ChangeProductThumbnail(int productId, string imageUrlStr)
        {
            var productImage = new ProductImage
            {
                ProductImageUrl = imageUrlStr,
                ProductId = productId,
                IsThumbnail = true
            };

            await ProductDao.Instance.ChangeProductThumbnail(productImage);
        }

        public async Task UpdateProductImage(int productId, List<ImageList> imageList)
        {
            var product = await ProductDao.Instance.GetProduct(productId);
            bool isFirstImage = true;

            var productImages = new List<ProductImage>();

            foreach (var imageUrl in imageList)
            {
                var productImage = new ProductImage
                {
                    ProductId = productId,
                    ProductImageUrl = imageUrl.Url,
                    IsThumbnail = isFirstImage
                };

                productImages.Add(productImage);
                isFirstImage = false;
            }

            await ProductDao.Instance.AddProductImages(productId, productImages);
        }

        public async Task UpdateProductTerm(int productId, IEnumerable<CreateProductTermDto> productTermDtos)
        {
            var product = await ProductDao.Instance.GetProductDetail(productId);

            var productTerms = new List<ProductTerm>();

            foreach (var termDto in productTermDtos)
            {
                var term = new ProductTerm
                {
                    ProductId = product.ProductId,
                    Title = termDto.Title,
                    Content = termDto.Content
                };

                productTerms.Add(term);
            }

            await ProductDao.Instance.UpdateProductTerm(product, productTerms);
        }

        public async Task<IEnumerable<ProductDto>> GetTop8LatestProductList()
        {
            var products = await ProductDao.Instance.GetTop8LatestProducts();
            if (!products.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }

            return [];
        }

        public async Task<IEnumerable<ProductReviewDto>> GetProductReviews(List<int> productIds)
        {
            var productReviewList = new List<ProductReviewDto>();

            foreach (var productId in productIds)
            {
                var product = await ProductDao.Instance.GetProductWithSerialProductNumberAndProductImages(productId);

                if (product != null)
                {
                    var productReview = _mapper.Map<ProductReviewDto>(product);

                    var thumbnailUrl = product.ProductImages
                        .FirstOrDefault(p => p.IsThumbnail == true)?.ProductImageUrl ?? string.Empty;
                    productReview.ThumbnailUrl = thumbnailUrl;

                    var serialNumberProducts = await SerialNumberProductDao.Instance.GetSerialNumberProductsByProductIdAndStatus(productReview.ProductId, SerialNumberProductStatusEnum.Available.ToString());
                    if (!serialNumberProducts.IsNullOrEmpty())
                    {
                        productReview.Quantity = serialNumberProducts.Count();

                        var prices = serialNumberProducts
                            .Select(s => s.ActualRentPrice ?? 0)
                            .OrderBy(s => s)
                            .ToList();
                        productReview.RentPrices = prices;

                        productReviewList.Add(productReview);
                    }
                }
            }

            return productReviewList;
        }
    }
}
