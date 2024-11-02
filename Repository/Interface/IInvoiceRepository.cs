using DTOs.Invoice;
using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IInvoiceRepository
    {
        Task<InvoiceDto?> AddTransactionToInvoice(TransactionReturn transactionReturn, string invoiceId);
        Task<IEnumerable<InvoiceDto>> GetAllInvoices();
        Task<InvoiceDto> GetInvoice(string invoiceId);
        Task UpdateInvoice(InvoiceDto invoice);
        Task<object?> GetInvoiceDetail(string invoiceId);
        Task<(InvoiceDto DepositInvoice, InvoiceDto RentalInvoice)> InitInvoices(RentingRequestDto rentingRequest);
        Task UpdateInvoiceStatus(string invoiceId, string status);
    }
}
