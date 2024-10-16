using AutoMapper;
using Common;
using Common.Enum;
using DTOs.Product;
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
        private readonly IMapper _mapper;

        public SerialNumberProductService(ISerialNumberProductRepository serialNumberProductRepository, IProductRepository productRepository, IMapper mapper)
        {
            _serialNumberProductRepository = serialNumberProductRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto dto, int accountId)
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

            await _serialNumberProductRepository.CreateSerialNumberProduct(dto, productDetail.ComponentProductList, (double)productDetail.RentPrice, accountId);

            var product = _mapper.Map<ProductDto>(productDetail);

            if (product.Status == ProductStatusEnum.NoSerialMachine.ToString())
            {
                product.Status = ProductStatusEnum.Active.ToString();
            }

            await _productRepository.UpdateProduct(product);
        }

        public async Task Delete(string serialNumber)
        {
            var serialNumberProduct = await _serialNumberProductRepository.GetSerialNumberProduct(serialNumber);

            if (serialNumberProduct != null)
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductNotFound);

            }
            if (string.IsNullOrEmpty(serialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberRequired);
            }

            if (await _serialNumberProductRepository.IsSerialNumberProductHasContract(serialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductHasContract);
            }

            await _serialNumberProductRepository.Delete(serialNumber);


            //var serialProductList = await _productRepository.GetProductNumberList()
        }

        public async Task<IEnumerable<SerialNumberProductLogDto>> GetDetailLog(string serialNumber)
        {
            var serialNumberProduct = await _serialNumberProductRepository.GetSerialNumberProduct(serialNumber);

            if (serialNumberProduct == null)
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductNotFound);
            }

            return await _serialNumberProductRepository.GetSerialNumberProductLog(serialNumber);
        }

        public async Task<IEnumerable<ProductComponentStatusDto>> GetSerialNumberComponentStatus(string serialNumber)
        {
            var serialNumberProduct = await _serialNumberProductRepository.GetSerialNumberProduct(serialNumber);

            if (serialNumberProduct == null)
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductNotFound);
            }

            return await _serialNumberProductRepository.GetProductComponentStatus(serialNumber);
        }

        public async Task<IEnumerable<SerialNumberProductOptionDto>> GetSerialProductNumbersAvailableForRenting(string rentingRequestId)
        {
            var serialNumberProducts = await _serialNumberProductRepository.GetSerialProductNumbersAvailableForRenting(rentingRequestId);

            if (serialNumberProducts.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.NoAvailableSerailNumberProductForRenting);
            }

            return serialNumberProducts;
        }

        public async Task Update(string serialNumber, SerialNumberProductUpdateDto serialNumberProductUpdateDto)
        {
            if (string.IsNullOrEmpty(serialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberRequired);
            }

            if (!await _serialNumberProductRepository.IsSerialNumberExist(serialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductNotFound);
            }

            await _serialNumberProductRepository.Update(serialNumber, serialNumberProductUpdateDto);
        }

        public async Task UpdateStatus(string serialNumber, string status)
        {
            if (string.IsNullOrEmpty(serialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberRequired);
            }

            if (!await _serialNumberProductRepository.IsSerialNumberExist(serialNumber))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.SerialNumberProductNotFound);
            }

            if (!status.Equals(SerialNumberProductStatusEnum.Maintenance.ToString()) ||
               !status.Equals(SerialNumberProductStatusEnum.Locked.ToString()) ||
               !status.Equals(SerialNumberProductStatusEnum.Available.ToString()))
            {
                throw new ServiceException(MessageConstant.SerialNumberProduct.StatusCannotSet);
            }

            await _serialNumberProductRepository.UpdateStatus(serialNumber, status);
        }
    }
}
