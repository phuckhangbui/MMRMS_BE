using DTOs;

namespace Service.Interface
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAll();
    }
}
