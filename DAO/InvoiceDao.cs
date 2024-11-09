﻿using BusinessObject;
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

        public async Task<int> GetTotalInvoiceByDate(DateTime date)
        {
            using var context = new MmrmsContext();
            return await context.Invoices
                .Where(r => r.DateCreate.HasValue && r.DateCreate.Value.Date == date.Date)
                .CountAsync();
        }

        public async Task<(Invoice depositInvoice, Invoice rentalInvoice)> GetInvoicesByRentingRequest(string rentingRequestId)
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

            var depositInvoice = invoices.FirstOrDefault(i => i.Type.Equals(InvoiceTypeEnum.Deposit.ToString()));
            var rentalInvoice = invoices.FirstOrDefault(i => i.Type.Equals(InvoiceTypeEnum.Rental.ToString()));

            return (depositInvoice, rentalInvoice);
        }
    }
}
