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

        public async Task ScheduleNextRentalPayment(string rentingRequestId)
        {
            using var context = new MmrmsContext();

            var contracts = await context.Contracts
                .Include(c => c.ContractPayments)
                .Where(c => c.RentingRequestId.Equals(rentingRequestId))
                .ToListAsync();

            foreach (var contract in contracts)
            {
                foreach (var contractPayment in contract.ContractPayments)
                {
                    var nextContractPayment = await context.ContractPayments
                        .Where(cp => cp.ContractId == contractPayment.ContractId &&
                                     cp.Status == ContractPaymentStatusEnum.Pending.ToString() &&
                                     cp.Type == ContractPaymentTypeEnum.Rental.ToString() &&
                                     cp.DateFrom > contractPayment.DateFrom)
                        .OrderBy(cp => cp.DateFrom)
                        .FirstOrDefaultAsync();

                    if (nextContractPayment != null)
                    {
                        DateTime oneWeekBefore = nextContractPayment.DateFrom.Value.AddDays(-7);
                        TimeSpan delayToStart = oneWeekBefore - DateTime.Now;

                        SheduleInvoiceGeneration(nextContractPayment.ContractPaymentId, delayToStart);
                    }
                    else
                    {
                        ScheduleContractCompletion(contractPayment.Contract);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }

        private void ScheduleContractCompletion(Contract contract)
        {
            ILogger<BackgroundImpl> logger = new LoggerFactory().CreateLogger<BackgroundImpl>();
            var backgroundImpl = new BackgroundImpl(logger);

            DateTime contractEndDate = (DateTime)contract.DateEnd;
            TimeSpan delayToStart = contractEndDate - DateTime.Now;
            backgroundImpl.CompleteContractOnTimeJob(contract.ContractId, delayToStart);
        }

        private void SheduleInvoiceGeneration(int contractPaymentId, TimeSpan delayToStart)
        {
            ILogger<BackgroundImpl> logger = new LoggerFactory().CreateLogger<BackgroundImpl>();
            var backgroundImpl = new BackgroundImpl(logger);

            backgroundImpl.GenerateInvoiceJob(contractPaymentId, delayToStart);
        }

        public async Task<bool> IsDepositAndFirstRentalPaid(string rentingRequestId)
        {
            using var context = new MmrmsContext();

            var rentingRequest = await context.RentingRequests
                .Where(r => r.RentingRequestId.Equals(rentingRequestId))
                .Include(r => r.Contracts)
                .FirstOrDefaultAsync();

            bool isAllPaid = true;
            foreach (var contract in rentingRequest.Contracts)
            {
                var depositPaid = await context.ContractPayments
                    .AnyAsync(cp => cp.ContractId.Equals(contract.ContractId) &&
                                    cp.Type.Equals(ContractPaymentTypeEnum.Deposit.ToString()) &&
                                    cp.Status.Equals(ContractPaymentStatusEnum.Paid.ToString()));

                var firstRentalPaid = await context.ContractPayments
                    .AnyAsync(cp => cp.ContractId.Equals(contract.ContractId) &&
                                    cp.Type.Equals(ContractPaymentTypeEnum.Rental.ToString()) &&
                                    cp.IsFirstRentalPayment == true &&
                                    cp.Status.Equals(ContractPaymentStatusEnum.Paid.ToString()));

                if (!depositPaid || !firstRentalPaid)
                {
                    isAllPaid = false;
                    break;
                }
            }

            return isAllPaid;
        }
    }
}
