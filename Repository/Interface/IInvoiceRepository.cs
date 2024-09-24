using DTOs;

namespace Repository.Interface
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<InvoiceDto>> GetAllInvoices();
    }
}
