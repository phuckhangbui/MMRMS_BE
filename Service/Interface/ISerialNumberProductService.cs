using DTOs.SerialNumberProduct;

namespace Service.Interface
{
    public interface ISerialNumberProductService
    {
        Task CreateSerialNumberProduct(SerialNumberProductCreateRequestDto dto);



    }
}
