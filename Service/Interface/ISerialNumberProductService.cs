using DTOs.SerialNumberProduct;

namespace Service.Interface
{
    public interface ISerialNumberProductService
    {
        Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto dto, int accountId);
        Task Delete(string serialNumber);
        Task Update(string serialNumber, SerialNumberProductUpdateDto serialNumberProductUpdateDto);
        Task UpdateStatus(string serialNumber, string status);
        Task<IEnumerable<SerialNumberProductOptionDto>> GetSerialProductNumbersAvailableForRenting(string rentingRequestId);
        Task<IEnumerable<SerialNumberProductLogDto>> GetDetailLog(string serialNumber);
        Task<IEnumerable<ProductComponentStatusDto>> GetSerialNumberComponentStatus(string serialNumber);
    }
}
