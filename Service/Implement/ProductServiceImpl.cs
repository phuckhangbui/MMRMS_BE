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

        public ProductServiceImpl(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
    }
}
