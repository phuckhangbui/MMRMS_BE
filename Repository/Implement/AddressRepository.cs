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

        public async Task<bool> IsAddressValid(int addressId, int accountId)
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
            //address.IsDelete = false;

            await AddressDao.Instance.CreateAsync(address);
        }

        public async Task<bool> DeleteAddress(int addressId)
        {
            var address = await AddressDao.Instance.GetAddressById(addressId);
            if (address != null)
            {
                await AddressDao.Instance.RemoveAsync(address);

                return true;
            }

            return false;
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

        public async Task<AddressDto?> UpdateAddress(int accountId, int addressId, AddressRequestDto addressRequestDto)
        {
            var addresses = await AddressDao.Instance.GetAddressesForCustomer(accountId);
            var currentAddress = addresses.FirstOrDefault(a => a.AddressId == addressId);

            if (currentAddress != null)
            {
                currentAddress.AddressBody = addressRequestDto.AddressBody;
                currentAddress.Coordinates = addressRequestDto.Coordinates;
                currentAddress.City = addressRequestDto.City;
                currentAddress.District = addressRequestDto.District;

                currentAddress = await AddressDao.Instance.UpdateAsync(currentAddress);
                return _mapper.Map<AddressDto>(currentAddress);
            }

            return null;
        }
    }
}
