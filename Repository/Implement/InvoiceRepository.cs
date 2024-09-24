using AutoMapper;
using DAO;
using DTOs;
using Repository.Interface;

namespace Repository.Implement
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IMapper _mapper;

        public InvoiceRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoices()
        {
            var invoices = await InvoiceDao.Instance.GetInvoices();

            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }
    }
}
