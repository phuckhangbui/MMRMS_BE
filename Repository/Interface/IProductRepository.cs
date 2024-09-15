using DTOs.Product;

namespace Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductList();

        Task<bool> IsProductExisted(int productId);

        Task<DisplayProductDetailDto> GetProductDetail(int productId);

        Task<IEnumerable<SerialProductNumberDto>> GetProductNumberList(int productId);
    }
}
