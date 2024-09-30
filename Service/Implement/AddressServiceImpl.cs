using Common;
using DTOs.Address;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class AddressServiceImpl : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressServiceImpl(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task CreateAddressForCustomer(int customerId, AddressRequestDto addressRequestDto)
        {
            await _addressRepository.CreateAddressForCustomer(customerId, addressRequestDto);
        }

        public async Task<IEnumerable<AddressDto>> GetAddressesForCustomer(int customerId)
        {
            var addresses = await _addressRepository.GetAddressesForCustomer(customerId);

            if (addresses.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Address.AddressListEmpty);
            }

            return addresses;
        }
    }
}
