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

        public async Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto createSerialProductNumberDto, IEnumerable<ComponentProductDto> componentProductList)
        {
            var serialProduct = new SerialNumberProduct
            {
                SerialNumber = createSerialProductNumberDto.SerialNumber,
                ProductId = createSerialProductNumberDto.ProductId,
                DateCreate = DateTime.Now,
                RentTimeCounter = 0,
                IsDelete = false,
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
        }

        public async Task<IEnumerable<SerialProductNumberDto>> GetSerialProductNumbersAvailableForRenting(string hiringRequestId)
        {
            var hiringRequest = await HiringRequestDao.Instance.GetHiringRequestById(hiringRequestId);

            var allSerialNumberProducts = new List<SerialNumberProduct>();

            foreach (var hiringRequestProductDetail in hiringRequest.HiringRequestProductDetails)
            {
                var serialNumberProducts = await SerialNumberProductDao.Instance
                    .GetSerialNumberProductsByProductIdAndStatus((int)hiringRequestProductDetail.ProductId, SerialNumberProductStatus.Available.ToString());

                allSerialNumberProducts.AddRange(serialNumberProducts);
            }

            return _mapper.Map<IEnumerable<SerialProductNumberDto>>(allSerialNumberProducts);
        }

        public async Task<bool> IsSerialNumberExist(string serialNumber)
        {
            return await SerialNumberProductDao.Instance.IsSerialNumberExisted(serialNumber);
        }
    }
}
