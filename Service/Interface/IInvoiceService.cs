using DTOs;
using DTOs.Invoice;

namespace Service.Interface
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAll();
        Task<IEnumerable<InvoiceDto>> GetCustomerInvoices(int customerId);
        Task<string> GetPaymentUrl(int customerId, string invoiceId, UrlDto urlDto);
        Task<bool> PostTransactionProcess(int customerId, string invoiceId);
        Task<object?> GetInvoiceDetail(string invoiceId);
        Task<InvoiceDto> CreateRefundInvoice(int accountId, RefundInvoiceRequestDto refundInvoiceRequestDto);
    }
}
