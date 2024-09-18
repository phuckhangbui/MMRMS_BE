﻿using AutoMapper;
using BusinessObject;
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

            return productDetail;
        }

        public async Task<IEnumerable<SerialProductNumberDto>> GetProductNumberList(int productId)
        {
            var product = await ProductDao.Instance.GetProductWithSerialProductNumber(productId);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<SerialProductNumberDto>>(product.SerialNumberProducts);
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
                    componentProduct.Status = "Active";
                    componentProducts.Add(componentProduct);
                }
            }

            product.ComponentProducts = componentProducts;

            product.Quantity = 0;
            product.DateCreate = DateTime.Now;
            product.IsDelete = false;
            product.Status = "NoSerialMachine";

            product = await ProductDao.Instance.CreateProduct(product, createProductDto.NewComponentList);


            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> IsProductExisted(int productId)
        {
            return await ProductDao.Instance.IsProductExisted(productId);
        }
    }
}
