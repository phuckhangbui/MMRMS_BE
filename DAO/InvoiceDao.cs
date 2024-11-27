using BusinessObject;
using Common.Enum;
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
                return await context.Invoices.Include(i => i.AccountPaid).OrderByDescending(i => i.DateCreate)
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

        public async Task<int> GetLastestInvoiceByDate(DateTime date)
        {
            using (var context = new MmrmsContext())
            {
                var ids = await context.Invoices
                    .Where(t => t.DateCreate.HasValue && t.DateCreate.Value.Date == date.Date)
                    .Where(t => t.InvoiceId != null)
                    .Select(t => t.InvoiceId)
                    .ToListAsync();

                if (!ids.Any())
                {
                    return 0;
                }

                var latestId = ids
                     .Select(id =>
                     {
                         int startIndex = id.IndexOf("NO") + 2;
                         return int.Parse(id.Substring(startIndex));
                     })
                     .OrderByDescending(id => id)
                     .FirstOrDefault();

                return latestId;
            }
        }

        public async Task<Invoice> GetInvoicesByRentingRequest(string rentingRequestId)
        {
            using var context = new MmrmsContext();

            var invoices = await context.Invoices
                    .Where(i => context.ContractPayments
                        .Where(cp => context.Contracts
                            .Where(c => c.RentingRequestId == rentingRequestId)
                            .Select(c => c.ContractId)
                            .Contains(cp.ContractId))
                        .Select(cp => cp.InvoiceId)
                        .Contains(i.InvoiceId))
                    .OrderBy(i => i.DateCreate)
                    .ToListAsync();

            //var depositInvoice = invoices.FirstOrDefault(i => i.Type.Equals(InvoiceTypeEnum.Deposit.ToString()) && i.Status.Equals(InvoiceStatusEnum.Pending.ToString()));
            var rentalInvoice = invoices.FirstOrDefault(i => i.Type.Equals(InvoiceTypeEnum.Rental.ToString()) && i.Status.Equals(InvoiceStatusEnum.Pending.ToString()));

            //return (depositInvoice, rentalInvoice);
            return rentalInvoice;
        }

        public async Task<double> GetTotalMoneyInRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            using var context = new MmrmsContext();
            IQueryable<Invoice> query = context.Invoices.Where(i => i.Status.Equals(InvoiceStatusEnum.Paid.ToString()));

            if (startDate.HasValue)
            {
                query = query.Where(i => i.DatePaid > startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(i => i.DatePaid < endDate.Value);
            }

            return (double)await query.SumAsync(i => i.Amount);
        }

        public async Task<IEnumerable<Invoice>> GetCustomerInvoices(int customerId)
        {
            using var context = new MmrmsContext();

            var invoices = await context.Invoices
                .Include(i => i.ContractPayments)
                    .ThenInclude(cp => cp.Contract)
                .Include(i => i.AccountPaid)
                .ToListAsync();

            var customerInvoices = invoices
                .Where(i => i.AccountPaidId == customerId ||
                    i.ContractPayments.Any(cp => cp.Contract.AccountSignId == customerId &&
                        i.Type.Equals(InvoiceTypeEnum.Refund.ToString())))
                .OrderByDescending(i => i.DateCreate)
                .Distinct()
                .ToList();

            return customerInvoices;
        }
    }
}
