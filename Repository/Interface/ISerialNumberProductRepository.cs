using DTOs.Product;
using DTOs.SerialNumberProduct;

namespace Repository.Interface
{
    public interface ISerialNumberProductRepository
    {
        Task<bool> CheckSerialNumberProductsValidToRent(List<SerialNumberProductRentRequestDto> serialNumberProductRentRequestDtos);
        Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto createSerialProductNumberDto, IEnumerable<ComponentProductDto> componentProductList);
        Task<bool> IsSerialNumberExist(string serialNumber);
        Task<IEnumerable<SerialProductNumberDto>> GetSerialProductNumbersAvailableForRenting(string rentingRequestId);
    }
}
