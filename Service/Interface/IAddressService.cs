using DTOs.Address;

namespace Service.Interface
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetAddressesForCustomer(int customerId);
        Task CreateAddressForCustomer(int customerId, AddressRequestDto addressRequestDto);
    }
}
