using BusinessObject;
using Common.Enum;
using DTOs.Invoice;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAO
{
    public class InvoiceDao : BaseDao<Invoice>
    {
        private static InvoiceDao instance = null;
        private static readonly object instacelock = new object();

        private InvoiceDao()
        {

        }

        public static InvoiceDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InvoiceDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<Invoice>> GetInvoices()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Invoices.Include(i => i.AccountPaid)
                    .ToListAsync();
            }
        }

        public async Task<Invoice> GetInvoice(string invoiceId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Invoices.Include(i => i.AccountPaid)
                    .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
            }
        }

        //TODO
        public async Task<Invoice?> UpdateContractInvoice(TransactionReturn transactionReturn, string invoiceId)
        {
            using var context = new MmrmsContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var invoice = await context.Invoices
                    .Include(i => i.ContractPayments)
                        .ThenInclude(cp => cp.Contract)
                        .ThenInclude(c => c.RentingRequest)
                    .FirstOrDefaultAsync(i => i.InvoiceId.Equals(invoiceId));
                if (invoice == null) return null;

                invoice = UpdateInvoiceData(invoice, transactionReturn);

                foreach (var contractPayment in invoice.ContractPayments)
                {
                    contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();
                    contractPayment.CustomerPaidDate = invoice.DatePaid;

                    if (contractPayment.Contract == null || contractPayment.Contract.RentingRequest == null)
                    {
                        return null;
                    }

                    //Deposit invoice
                    if (contractPayment.Type.Equals(ContractPaymentTypeEnum.Deposit.ToString()))
                    {
                        contractPayment.Contract.Status = ContractStatusEnum.Signed.ToString();
                        contractPayment.Contract.RentingRequest.Status = RentingRequestStatusEnum.Signed.ToString();
                    }

                    //First rental invoice
                    else if (contractPayment.Type.Equals(ContractPaymentTypeEnum.Rental.ToString()) && contractPayment.IsFirstRentalPayment == true)
                    {
                        contractPayment.Contract.Status = ContractStatusEnum.Renting.ToString();
                        contractPayment.Contract.RentingRequest.Status = RentingRequestStatusEnum.Shipped.ToString();

                        if (contractPayment.Contract.RentingRequest.IsOnetimePayment == true)
                        {
                            ScheduleContractCompletion(contractPayment.Contract);
                        }
                    }

                    //Monthly rental invoice
                    else
                    {
                        var nextContractPayment = await context.ContractPayments
                            .Where(cp => cp.ContractId.Equals(contractPayment.ContractId) &&
                                cp.Status.Equals(ContractPaymentStatusEnum.Pending.ToString()) &&
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
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return invoice;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }

        private Invoice UpdateInvoiceData(Invoice invoice, TransactionReturn transactionReturn)
        {
            invoice.Status = InvoiceStatusEnum.Paid.ToString();
            invoice.PaymentMethod = InvoicePaymentTypeEnum.Digital.ToString();
            invoice.DigitalTransactionId = transactionReturn.Reference;
            invoice.DatePaid = transactionReturn.TransactionDate;

            var digitalTransaction = new DigitalTransaction
            {
                DigitalTransactionId = transactionReturn.Reference,
                AccountNumber = transactionReturn.AccountNumber,
                AccountName = transactionReturn.AccountName,
                BankCode = transactionReturn.BankCode,
                BankName = transactionReturn.BankName,
                Amount = transactionReturn.Amount,
                Description = transactionReturn.Description,
                TransactionDate = transactionReturn.TransactionDate,
            };

            invoice.DigitalTransaction = digitalTransaction;

            return invoice;
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
    }
}
