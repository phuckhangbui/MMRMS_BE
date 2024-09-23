using DTOs.SerialNumberProduct;

namespace Repository.Interface
{
    public interface ISerialNumberProductRepository
    {
        Task<bool> CheckSerialNumberProductsValidToRent(List<SerialNumberProductRentRequestDto> serialNumberProductRentRequestDtos);
    }
}
