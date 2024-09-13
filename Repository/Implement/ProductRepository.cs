using AutoMapper;
using DAO;
using DTOs.Product;
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
        public async Task<DisplayProductDetailDto> GetProductDetail(int productId)
        {
            var product = await ProductDao.Instance.GetProductDetail(productId);

            if (product == null)
            {
                return null;
            }

            var productDetail = _mapper.Map<DisplayProductDetailDto>(product);

            if (product.ProductComponentDetails != null && product.ProductComponentDetails.First().ComponentProduct != null)
            {
                List<ComponentProductDto> componentProductList = new List<ComponentProductDto>();

                foreach (var detail in product.ProductComponentDetails)
                {
                    var componentProductDto = _mapper.Map<ComponentProductDto>(detail.ComponentProduct);

                    componentProductList.Add(componentProductDto);
                }
                productDetail.ComponentProductList = componentProductList;
            }

            return productDetail;
        }

    }
}
