﻿using DTOs.Invoice;

namespace Repository.Interface
{
    public interface IInvoiceRepository
    {
        Task<InvoiceDto?> AddTransactionToInvoice(TransactionReturn transactionReturn, string invoiceId);
        Task<IEnumerable<InvoiceDto>> GetAllInvoices();
        Task<InvoiceDto> GetInvoice(string invoiceId);
        Task UpdateInvoice(InvoiceDto invoice);
        Task<object?> GetInvoiceDetail(string invoiceId);
        Task<InvoiceDto> CreateInvoice(double amount, string type, int accountPaidId, string note);
        Task CreateInvoice(string rentingRequestId);
        Task GenerateMonthlyInvoices(string rentingRequestId);
        Task UpdateInvoiceStatus(string invoiceId, string status);
    }
}
