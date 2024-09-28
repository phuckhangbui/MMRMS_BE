using DTOs.SerialNumberProduct;

namespace Service.Interface
{
    public interface ISerialNumberProductService
    {
        Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto dto);
        Task Delete(string serialNumber);
        Task Update(string serialNumber, SerialNumberProductUpdateDto serialNumberProductUpdateDto);
        Task UpdateStatus(string serialNumber, string status);
    }
}
