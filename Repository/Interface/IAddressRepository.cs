﻿using DTOs.Address;

namespace Repository.Interface
{
    public interface IAddressRepository
    {
        Task<bool> CheckAddressValid(int addressId, int accountId);
        Task<IEnumerable<AddressDto>> GetAddressesForCustomer(int customerId);
        Task CreateAddressForCustomer(int customerId, AddressRequestDto addressRequestDto);
    }
}
