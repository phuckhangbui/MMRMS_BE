using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.Address;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IMapper _mapper;

        public AddressRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CheckAddressValid(int addressId, int accountId)
        {
            var address = await AddressDao.Instance.GetAddressById(addressId);
            if (address == null || address.AccountId != accountId)
            {
                return false;
            }

            return true;
        }

        public async Task CreateAddressForCustomer(int customerId, AddressRequestDto addressRequestDto)
        {
            var address = _mapper.Map<Address>(addressRequestDto);

            address.AccountId = customerId;
            address.IsDelete = false;

            await AddressDao.Instance.CreateAsync(address);
        }

        public async Task<IEnumerable<AddressDto>> GetAddressesForCustomer(int customerId)
        {
            var addresses = await AddressDao.Instance.GetAddressesForCustomer(customerId);

            if (!addresses.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<AddressDto>>(addresses);
            }

            return [];
        }
    }
}
