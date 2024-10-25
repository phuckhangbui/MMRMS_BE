using DTOs;
using DTOs.Invoice;

namespace Service.Interface
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAll();
        Task<IEnumerable<InvoiceDto>> GetCustomerInvoice(int customerId);
        Task<string> GetPaymentUrl(int customerId, string invoiceId, UrlDto urlDto);
        Task<bool> PostTransactionProcess(int customerId, string invoiceId);
        Task<object?> GetInvoiceDetail(string invoiceId);
    }
}
