﻿using DTOs.Product;

namespace Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductList();

        Task<bool> IsProductExisted(int productId);
        Task<bool> IsProductExisted(string name);

        Task<DisplayProductDetailDto> GetProductDetail(int productId);

        Task<IEnumerable<SerialProductNumberDto>> GetProductNumberList(int productId);

        Task<ProductDto> CreateProduct(CreateProductDto createProductDto);
    }
}
