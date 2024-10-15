using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.Product;
using DTOs.RentingRequest;
using DTOs.SerialNumberProduct;
using Repository.Interface;

namespace Repository.Implement
{
    public class SerialNumberProductRepository : ISerialNumberProductRepository
    {
        private readonly IMapper _mapper;

        public SerialNumberProductRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CheckSerialNumberProductsValidToRent(List<SerialNumberProductRentRequestDto> serialNumberProductRentRequestDtos)
        {
            foreach (var serialNumberProductRentRequestDto in serialNumberProductRentRequestDtos)
            {
                var isSerialNumberProductValid = await SerialNumberProductDao.Instance.IsSerialNumberProductValidToRent(
                    serialNumberProductRentRequestDto.SerialNumber, serialNumberProductRentRequestDto.ProductId);

                if (!isSerialNumberProductValid)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CheckSerialNumberProductValidToRequest(
            List<NewRentingRequestProductDetailDto> rentingRequestProductDetailDtos,
            DateTime requestStartDate,
            int numberOfMonth)
        {
            foreach (var rentingRequestProductDetailDto in rentingRequestProductDetailDtos)
            {
                var isSerialNumberProductValid = await SerialNumberProductDao.Instance
                    .IsSerialNumberProductValidToRent(
                            rentingRequestProductDetailDto.ProductId,
                            rentingRequestProductDetailDto.Quantity,
                            requestStartDate,
                            numberOfMonth);

                if (!isSerialNumberProductValid)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto createSerialProductNumberDto, IEnumerable<ComponentProductDto> componentProductList, double price)
        {
            var serialProduct = new SerialNumberProduct
            {
                SerialNumber = createSerialProductNumberDto.SerialNumber,
                ProductId = createSerialProductNumberDto.ProductId,
                DateCreate = DateTime.Now,
                RentTimeCounter = 0,
                ActualRentPrice = price,
                Status = SerialNumberProductStatusEnum.Available.ToString()
            };

            IList<ProductComponentStatus> productComponentStatuses = new List<ProductComponentStatus>();

            foreach (var componentProduct in componentProductList)
            {
                var productComponentStatus = new ProductComponentStatus
                {
                    SerialNumber = createSerialProductNumberDto.SerialNumber,
                    ComponentId = componentProduct.ComponentProductId,
                    Quantity = componentProduct.Quantity,
                    Status = ProductComponentStatusEnum.Normal.ToString()
                };

                productComponentStatuses.Add(productComponentStatus);
            }

            if (productComponentStatuses.Count == 0)
            {
                serialProduct.ProductComponentStatuses = null;
            }
            else
                serialProduct.ProductComponentStatuses = productComponentStatuses;


            await SerialNumberProductDao.Instance.CreateAsync(serialProduct);




            var product = await ProductDao.Instance.GetProduct((int)serialProduct.ProductId);

            if (product.Status.Equals(ProductStatusEnum.NoSerialMachine))
            {
                product.Status = ProductStatusEnum.Active.ToString();
            }


            await ProductDao.Instance.UpdateAsync(product);
        }

        public async Task Delete(string serialNumber)
        {
            await SerialNumberProductDao.Instance.Delete(serialNumber);
        }

        public async Task<IEnumerable<SerialNumberProductOptionDto>> GetSerialProductNumbersAvailableForRenting(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(rentingRequestId);

            var allSerialNumberProducts = new List<SerialNumberProduct>();

            foreach (var rentingRequestProductDetail in rentingRequest.RentingRequestProductDetails)
            {
                var serialNumberProducts = await SerialNumberProductDao.Instance
                    .GetSerialNumberProductsByProductIdAndStatus((int)rentingRequestProductDetail.ProductId!, SerialNumberProductStatusEnum.Available.ToString());

                allSerialNumberProducts.AddRange(serialNumberProducts);
            }

            return _mapper.Map<IEnumerable<SerialNumberProductOptionDto>>(allSerialNumberProducts);
        }

        public async Task<bool> IsSerialNumberExist(string serialNumber)
        {
            return await SerialNumberProductDao.Instance.IsSerialNumberExisted(serialNumber);
        }

        public async Task<bool> IsSerialNumberProductHasContract(string serialNumber)
        {
            return await SerialNumberProductDao.Instance.IsSerialNumberInAnyContract(serialNumber);
        }

        public async Task Update(string serialNumber, SerialNumberProductUpdateDto serialNumberProductUpdateDto)
        {
            var serialNumberProduct = await SerialNumberProductDao.Instance.GetSerialNumberProduct(serialNumber);

            if (serialNumberProduct != null)
            {
                serialNumberProduct.ActualRentPrice = serialNumberProductUpdateDto.ActualRentPrice;
                serialNumberProduct.RentTimeCounter = serialNumberProductUpdateDto.RentTimeCounter;

                await SerialNumberProductDao.Instance.UpdateAsync(serialNumberProduct);
            }
        }

        public async Task UpdateStatus(string serialNumber, string status)
        {
            await SerialNumberProductDao.Instance.UpdateStatus(serialNumber, status);
        }
    }
}
