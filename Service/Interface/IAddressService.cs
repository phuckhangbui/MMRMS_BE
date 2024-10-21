using DTOs.Address;

namespace Service.Interface
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetAddressesForCustomer(int customerId);
        Task CreateAddressForCustomer(int customerId, AddressRequestDto addressRequestDto);
        Task<bool> UpdateAddress(int accountId, int addressId, AddressRequestDto addressRequestDto);
        Task<bool> DeleteAddress(int accountId, int addressId);
    }
}
