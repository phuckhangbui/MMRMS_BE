using AutoMapper;
using BusinessObject;
using DAO;
using DAO.Enum;
using DTOs.Product;
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

        public async Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto createSerialProductNumberDto, IEnumerable<ComponentProductDto> componentProductList, double price)
        {
            var serialProduct = new SerialNumberProduct
            {
                SerialNumber = createSerialProductNumberDto.SerialNumber,
                ProductId = createSerialProductNumberDto.ProductId,
                DateCreate = DateTime.Now,
                RentTimeCounter = 0,
                ActualRentPrice = price,
                Status = SerialNumberProductStatus.Available.ToString()
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

            product.Quantity += 1;

            await ProductDao.Instance.UpdateAsync(product);
        }

        public async Task Delete(string serialNumber)
        {
            await SerialNumberProductDao.Instance.Delete(serialNumber);
        }

        public async Task<IEnumerable<SerialNumberProductDto>> GetSerialProductNumbersAvailableForRenting(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(rentingRequestId);

            var allSerialNumberProducts = new List<SerialNumberProduct>();

            foreach (var rentingRequestProductDetail in rentingRequest.RentingRequestProductDetails)
            {
                var serialNumberProducts = await SerialNumberProductDao.Instance
                    .GetSerialNumberProductsByProductIdAndStatus((int)rentingRequestProductDetail.ProductId, SerialNumberProductStatus.Available.ToString());

                allSerialNumberProducts.AddRange(serialNumberProducts);
            }

            return _mapper.Map<IEnumerable<SerialNumberProductDto>>(allSerialNumberProducts);
        }

        public async Task<bool> IsSerialNumberExist(string serialNumber)
        {
            return await SerialNumberProductDao.Instance.IsSerialNumberExisted(serialNumber);
        }

        public async Task<bool> IsSerialNumberProductHasContract(string serialNumber)
        {
            return await SerialNumberProductDao.Instance.IsSerialNumberInAnyContract(serialNumber);
        }
    }
}
