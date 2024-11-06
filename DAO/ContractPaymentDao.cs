using BusinessObject;
using Common.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<string?> UpdateContractPayments(string invoiceId)
        {
            using var context = new MmrmsContext();

            var invoice = await context.Invoices
                .Include(i => i.ContractPayments)
                    .ThenInclude(cp => cp.Contract)
                    .ThenInclude(c => c.RentingRequest)
                .FirstOrDefaultAsync(i => i.InvoiceId.Equals(invoiceId));

            if (invoice == null) return null;

            string rentingRequestId = null;
            foreach (var contractPayment in invoice.ContractPayments)
            {
                contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();
                contractPayment.CustomerPaidDate = invoice.DatePaid;
                rentingRequestId = contractPayment.Contract.RentingRequestId;

                if (contractPayment.Contract == null || contractPayment.Contract.RentingRequest == null)
                    continue;

                await context.SaveChangesAsync();
            }

            return rentingRequestId;
        }

        public async Task<bool> IsDepositAndFirstRentalPaid(string rentingRequestId)
        {
            using var context = new MmrmsContext();
            var requiredPayments = await context.ContractPayments
                .Where(cp => cp.Contract.RentingRequestId == rentingRequestId &&
                     cp.Status == ContractPaymentStatusEnum.Paid.ToString() &&
                     (cp.Type == ContractPaymentTypeEnum.Deposit.ToString() ||
                      (cp.Type == ContractPaymentTypeEnum.Rental.ToString() && cp.IsFirstRentalPayment == true)))
                .GroupBy(cp => cp.ContractId)
                .ToListAsync();

            foreach (var contractGroup in requiredPayments)
            {
                var hasDepositPaid = contractGroup.Any(cp => cp.Type == ContractPaymentTypeEnum.Deposit.ToString());
                var hasFirstRentalPaid = contractGroup.Any(cp => cp.Type == ContractPaymentTypeEnum.Rental.ToString() && cp.IsFirstRentalPayment == true);

                if (!hasDepositPaid || !hasFirstRentalPaid)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
