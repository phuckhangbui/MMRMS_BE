using DTOs.Product;
using DTOs.RentingRequest;
using DTOs.SerialNumberProduct;

namespace Repository.Interface
{
    public interface ISerialNumberProductRepository
    {
        Task<bool> CheckSerialNumberProductsValidToRent(List<SerialNumberProductRentRequestDto> serialNumberProductRentRequestDtos);
        Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto createSerialProductNumberDto, IEnumerable<ComponentProductDto> componentProductList, double price);
        Task Delete(string serialNumber);
        Task<IEnumerable<SerialNumberProductOptionDto>> GetSerialProductNumbersAvailableForRenting(string rentingRequestId);
        Task<bool> IsSerialNumberExist(string serialNumber);
        Task<bool> IsSerialNumberProductHasContract(string serialNumber);
        Task Update(string serialNumber, SerialNumberProductUpdateDto serialNumberProductUpdateDto);
        Task UpdateStatus(string serialNumber, string status);
        Task<bool> CheckSerialNumberProductValidToRequest(List<NewRentingRequestProductDetailDto> rentingRequestProductDetailDtos);
    }
}
