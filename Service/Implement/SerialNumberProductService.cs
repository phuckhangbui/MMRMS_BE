﻿using Common;
using DTOs.SerialNumberProduct;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class SerialNumberProductService : ISerialNumberProductService
    {
        private readonly ISerialNumberProductRepository _serialNumberProductRepository;
        private readonly IProductRepository _productRepository;

        public SerialNumberProductService(ISerialNumberProductRepository serialNumberProductRepository, IProductRepository productRepository)
        {
            _serialNumberProductRepository = serialNumberProductRepository;
            _productRepository = productRepository;
        }

        public async Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto dto)
        {
            var productDetail = await _productRepository.GetProductDetail(dto.ProductId);

            if (productDetail == null)
            {
                throw new ServiceException(MessageConstant.Product.ProductNotFound);
            }

            var serialProductList = await _productRepository.GetProductNumberList(dto.ProductId);

            if (await _serialNumberProductRepository.IsSerialNumberExist(dto.SerialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductDuplicated);
            }

            if (productDetail.ComponentProductList.IsNullOrEmpty())
            {
                if (!dto.ForceWhenNoComponentInProduct)
                {
                    throw new ServiceException(MessageConstant.SerialNumberProduct.ProductHaveNoComponentAndIsForceSetToFalse);
                }
            }

            await _serialNumberProductRepository.CreateSerialNumberProduct(dto, productDetail.ComponentProductList);
        }
    }
}
