using AutoMapper;
using DAO;
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
                var isSerialNumberProductValid = await ProductNumberDao.Instance.IsSerialNumberProductValidToRent(
                    serialNumberProductRentRequestDto.SerialNumber, serialNumberProductRentRequestDto.ProductId);

                if (!isSerialNumberProductValid)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
