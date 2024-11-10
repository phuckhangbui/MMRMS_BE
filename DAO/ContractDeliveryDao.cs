using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ContractDeliveryDao : BaseDao<ContractDelivery>
    {
        private static ContractDeliveryDao instance = null;
        private static readonly object instacelock = new object();

        private ContractDeliveryDao()
        {

        }

        public static ContractDeliveryDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContractDeliveryDao();
                }
                return instance;
            }
        }

        public async Task<ContractDelivery> GetContractDeliveryById(int contractDeliveryId)
        {
            using var context = new MmrmsContext();
            return await context.ContractDeliveries
                .FirstOrDefaultAsync(c => c.ContractDeliveryId == contractDeliveryId);
        }
    }
}
