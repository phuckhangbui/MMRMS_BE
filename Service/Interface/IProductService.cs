using DTOs.Product;
using DTOs.SerialNumberProduct;

namespace Service.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductList();

        Task<DisplayProductDetailDto> GetProductDetailDto(int productId);

        Task<IEnumerable<SerialNumberProductDto>> GetSerialProductList(int productId);

        Task<ProductDto> CreateProduct(CreateProductDto createProductDto);

        Task DeleteProduct(int productId);
        Task UpdateProductStatus(int productId, string status);
        Task UpdateProductAttribute(int productId, IEnumerable<CreateProductAttributeDto> productAttributeDtos);
        Task UpdateProductDetail(int productId, UpdateProductDto updateProductDto);
        Task UpdateProductComponent(int productId, ComponentList productAttributeDtos);
        Task ChangeProductImages(int productId, List<ImageList> imageList);
    }
}
