using DTOs.Product;
using DTOs.SerialNumberProduct;

namespace Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductList();
        Task<IEnumerable<ProductReviewDto>> GetTop8LatestProductList();
        Task<bool> IsProductExisted(int productId);
        Task<bool> IsProductExisted(string name);
        Task<bool> IsProductModelExisted(string model);
        Task<DisplayProductDetailDto> GetProductDetail(int productId);
        Task<IEnumerable<SerialNumberProductDto>> GetProductNumberList(int productId);
        Task<ProductDto> CreateProduct(CreateProductDto createProductDto);
        Task<ProductDto> GetProduct(int productId);
        Task UpdateProduct(ProductDto productDto);
        Task DeleteProduct(int productId);
        Task UpdateProductAttribute(int productId, IEnumerable<CreateProductAttributeDto> productAttributeDtos);
        Task UpdateProductComponent(int productId, ComponentList productComponentDtos);
        Task ChangeProductThumbnail(int productId, string imageUrlStr);
        Task UpdateProductImage(int productId, List<ImageList> imageList);
        Task UpdateProductTerm(int productId, IEnumerable<CreateProductTermDto> productTermDtos);
    }
}
