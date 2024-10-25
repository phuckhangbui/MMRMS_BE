using BusinessObject;
using Common.Enum;
using DTOs.Invoice;
using Microsoft.EntityFrameworkCore;

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
                                contractPayment.Contract.Status = ContractStatusEnum.Signed.ToString();
                                contractPayment.Contract.RentingRequest.Status = RentingRequestStatusEnum.Signed.ToString();
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
