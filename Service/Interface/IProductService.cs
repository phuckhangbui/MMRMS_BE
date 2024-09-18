using DTOs.Product;

namespace Service.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductList();

        Task<DisplayProductDetailDto> GetProductDetailDto(int productId);

        Task<IEnumerable<SerialProductNumberDto>> GetSerialProductList(int productId);

        Task<ProductDto> CreateProduct(CreateProductDto createProductDto);

    }
}
