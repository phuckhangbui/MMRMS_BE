using DAO;
using Repository.Interface;

namespace Repository.Implement
{
    public class AddressRepository : IAddressRepository
    {
        public async Task<bool> CheckAddressValid(int addressId, int accountId)
        {
            var address = await AddressDao.Instance.GetAddressById(addressId);
            if (address == null || address.AccountId != accountId)
            {
                return false;
            }

            return true;
        }
    }
}
