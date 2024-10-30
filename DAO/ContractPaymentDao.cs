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

        public async Task UpdateRentalContractPayment(string invoiceId)
        {
            using var context = new MmrmsContext();

            var invoice = await context.Invoices
                    .Include(i => i.ContractPayments)
                        .ThenInclude(cp => cp.Contract)
                        .ThenInclude(c => c.RentingRequest)
                    .FirstOrDefaultAsync(i => i.InvoiceId.Equals(invoiceId));

            string rentingRequestId = null;
            foreach (var contractPayment in invoice.ContractPayments)
            {
                contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();
                contractPayment.CustomerPaidDate = invoice.DatePaid;
                rentingRequestId = contractPayment.Contract.RentingRequestId;

                if (contractPayment.Contract == null || contractPayment.Contract.RentingRequest == null)
                {
                    continue;
                }

                var isDepositPaymentPaid = await context.ContractPayments
                        .AnyAsync(cp => cp.ContractId.Equals(contractPayment.ContractId) &&
                                        cp.Type.Equals(ContractPaymentTypeEnum.Deposit.ToString()) &&
                                        cp.Status.Equals(ContractPaymentStatusEnum.Paid.ToString()));
                if (isDepositPaymentPaid)
                {
                    contractPayment.Contract.Status = ContractStatusEnum.Signed.ToString();
                    contractPayment.Contract.DateSign = invoice.DatePaid;

                    await context.SaveChangesAsync();
                }

                //
                var nextContractPayment = await context.ContractPayments
                    .Where(cp => cp.ContractId.Equals(contractPayment.ContractId) &&
                        cp.Status.Equals(ContractPaymentStatusEnum.Pending.ToString()) &&
                        cp.Type.Equals(ContractPaymentTypeEnum.Rental.ToString()) &&
                        cp.DateFrom > contractPayment.DateFrom)
                    .OrderBy(cp => cp.DateFrom)
                    .FirstOrDefaultAsync();

                if (nextContractPayment != null)
                {
                    DateTime oneWeekBefore = contractPayment.DateFrom.Value.AddDays(-7);
                    TimeSpan delayToStart = oneWeekBefore - DateTime.Now;

                    SheduleInvoiceGeneration(contractPayment.ContractPaymentId, delayToStart);
                }
                else
                {
                    ScheduleContractCompletion(contractPayment.Contract);
                }
            }

            await UpdateRentingRequest(context, rentingRequestId);
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

        public async Task UpdateDepositContractPayment(string invoiceId)
        {
            using var context = new MmrmsContext();

            var invoice = await context.Invoices
                    .Include(i => i.ContractPayments)
                        .ThenInclude(cp => cp.Contract)
                        .ThenInclude(c => c.RentingRequest)
                    .FirstOrDefaultAsync(i => i.InvoiceId.Equals(invoiceId));
            //if (invoice == null) return null;

            string rentingRequestId = null;
            foreach (var contractPayment in invoice.ContractPayments)
            {
                contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();
                contractPayment.CustomerPaidDate = invoice.DatePaid;
                rentingRequestId = contractPayment.Contract.RentingRequestId;

                if (contractPayment.Contract == null || contractPayment.Contract.RentingRequest == null)
                {
                    continue;
                }

                var isFirstRentalPaymentPaid = await context.ContractPayments
                    .AnyAsync(cp => cp.ContractId.Equals(contractPayment.ContractId) &&
                                 cp.Type.Equals(ContractPaymentTypeEnum.Rental.ToString()) &&
                                 cp.Status.Equals(ContractPaymentStatusEnum.Paid.ToString()) &&
                                 cp.IsFirstRentalPayment == true);
                if (isFirstRentalPaymentPaid)
                {
                    contractPayment.Contract.Status = ContractStatusEnum.Signed.ToString();
                    contractPayment.Contract.DateSign = invoice.DatePaid;

                    await context.SaveChangesAsync();
                }
            }

            await context.SaveChangesAsync();

            //return invoice;
            await UpdateRentingRequest(context, rentingRequestId);
        }

        private async Task UpdateRentingRequest(MmrmsContext context, string rentingRequestId)
        {
            //using var context = new MmrmsContext();
            var rentingRequest = await context.RentingRequests
                .Where(r => r.RentingRequestId.Equals(rentingRequestId))
                .Include(r => r.Contracts)
                .FirstOrDefaultAsync();

            bool allPaid = true;
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
                    allPaid = false;
                    break;
                }
            }


            if (allPaid)
            {
                rentingRequest.Status = RentingRequestStatusEnum.Signed.ToString();

                await context.SaveChangesAsync();
            }
        }
    }
}
