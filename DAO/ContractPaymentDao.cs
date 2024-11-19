using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ContractPaymentDao : BaseDao<ContractPayment>
    {
        private static ContractPaymentDao instance = null;
        private static readonly object instacelock = new object();

        private ContractPaymentDao()
        {

        }

        public static ContractPaymentDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContractPaymentDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<ContractPayment>> GetContractPaymentsByInvoiceId(string invoiceId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.ContractPayments
                    .Where(cp => cp.InvoiceId.Equals(invoiceId))
                    .Include(cp => cp.Contract)
                    .ToListAsync();
            }
        }

        public async Task<ContractPayment?> GetContractPaymentById(int contractPaymentId)
        {
            using var context = new MmrmsContext();
            return await context.ContractPayments
                .FirstOrDefaultAsync(cp => cp.ContractPaymentId == contractPaymentId);
        }
    }
}
