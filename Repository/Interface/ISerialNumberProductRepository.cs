using DTOs.Product;
using DTOs.RentingRequest;
using DTOs.SerialNumberProduct;

namespace Repository.Interface
{
    public interface ISerialNumberProductRepository
    {
        Task<bool> CheckSerialNumberProductsValidToRent(List<SerialNumberProductRentRequestDto> serialNumberProductRentRequestDtos);
        Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto createSerialProductNumberDto, IEnumerable<ComponentProductDto> componentProductList, double price, int accountId);
        Task Delete(string serialNumber);
        Task<IEnumerable<SerialNumberProductOptionDto>> GetSerialProductNumbersAvailableForRenting(string rentingRequestId);
        Task<bool> IsSerialNumberExist(string serialNumber);
        Task<bool> IsSerialNumberProductHasContract(string serialNumber);
        Task Update(string serialNumber, SerialNumberProductUpdateDto serialNumberProductUpdateDto);
        Task<bool> CheckSerialNumberProductValidToRequest(NewRentingRequestDto newRentingRequestDto);
        Task UpdateStatus(string serialNumber, string status, int staffId);
        Task<SerialNumberProductDto> GetSerialNumberProduct(string serialNumber);
        Task<IEnumerable<SerialNumberProductLogDto>> GetSerialNumberProductLog(string serialNumber);
        Task<IEnumerable<ProductComponentStatusDto>> GetProductComponentStatus(string serialNumber);
    }
}
