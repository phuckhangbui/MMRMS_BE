﻿using DTOs.Invoice;

namespace Repository.Interface
{
    public interface IInvoiceRepository
    {
        Task AddTransactionToInvoice(TransactionReturn transactionReturn, string invoiceId);
        Task<IEnumerable<InvoiceDto>> GetAllInvoices();
        Task<InvoiceDto> GetInvoice(string invoiceId);
        Task UpdateInvoice(InvoiceDto invoice);
    }
}
