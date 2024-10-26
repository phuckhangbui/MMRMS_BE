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
        public async Task<Invoice> UpdateContractInvoice(TransactionReturn transactionReturn, string invoiceId)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var invoice = await context.Invoices
                            .Include(i => i.ContractPayments)
                                .ThenInclude(cp => cp.Contract)
                                .ThenInclude(c => c.RentingRequest)
                            .FirstOrDefaultAsync(i => i.InvoiceId.Equals(invoiceId));

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

                        foreach (var contractPayment in invoice.ContractPayments)
                        {
                            contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();
                            contractPayment.CustomerPaidDate = invoice.DatePaid;

                            if (contractPayment.Contract != null && contractPayment.Contract.RentingRequest != null)
                            {
                                //Deposit invoice
                                if (contractPayment.Type.Equals(ContractPaymentTypeEnum.Deposit.ToString()))
                                {
                                    contractPayment.Contract.Status = ContractStatusEnum.Signed.ToString();
                                    contractPayment.Contract.RentingRequest.Status = RentingRequestStatusEnum.Signed.ToString();
                                }

                                //First rental invoice
                                if (contractPayment.Type.Equals(ContractPaymentTypeEnum.Rental.ToString()) && contractPayment.IsFirstRentalPayment == true)
                                {
                                    contractPayment.Contract.Status = ContractStatusEnum.Renting.ToString();
                                    contractPayment.Contract.RentingRequest.Status = RentingRequestStatusEnum.Shipped.ToString();

                                    //Background
                                    ILogger<BackgroundImpl> logger = new LoggerFactory().CreateLogger<BackgroundImpl>();
                                    var backgroundImpl = new BackgroundImpl(logger, new RentingRequestDao(), new ContractDao());

                                    DateTime contractEndDate = (DateTime)contractPayment.Contract.DateEnd;
                                    TimeSpan delayToStart = contractEndDate - DateTime.Now;
                                    backgroundImpl.ScheduleCompleteContractOnTimeJob(contractPayment.Contract.ContractId, delayToStart);
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
            }
        }
    }
}
