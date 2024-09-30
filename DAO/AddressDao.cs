using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class AddressDao : BaseDao<Address>
    {
        private static AddressDao instance = null;
        private static readonly object instacelock = new object();

        private AddressDao()
        {

        }

        public static AddressDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AddressDao();
                }
                return instance;
            }
        }

        public async Task<Address> GetAddressById(int addressId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Addresses
                    .FirstOrDefaultAsync(a => a.AddressId == addressId);
            }
        }

        public async Task<IEnumerable<Address>> GetAddressesForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Addresses
                    .Where(a => a.AccountId == customerId)
                    .ToListAsync();
            }
        }
    }
}
