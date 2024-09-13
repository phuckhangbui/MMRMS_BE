using DTOs.Product;

namespace Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductList();

        Task<DisplayProductDetailDto> GetProductDetail(int productId);
    }
}
