using DTOs;

namespace Service.Interface
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAll();
        Task<string> GetPaymentUrl(int customerId, string invoiceId, UrlDto urlDto);
        Task PostTransactionProcess(int customerId, string invoiceId);
    }
}
