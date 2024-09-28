using DTOs.Product;
using DTOs.RentingRequest;
using DTOs.SerialNumberProduct;

namespace Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductList();
        Task<bool> IsProductExisted(int productId);
        Task<bool> IsProductExisted(string name);
        Task<bool> IsProductModelExisted(string model);
        Task<DisplayProductDetailDto> GetProductDetail(int productId);
        Task<IEnumerable<SerialProductNumberDto>> GetProductNumberList(int productId);
        Task<ProductDto> CreateProduct(CreateProductDto createProductDto);
        Task<ProductDto> GetProduct(int productId);
        Task UpdateProduct(ProductDto productDto);
        Task DeleteProduct(int productId);
        Task<bool> CheckProductValidToRent(List<RentingRequestProductDetailDto> rentingRequestProductDetails);
    }
}
