using DTOs;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPayOSService _payOSService;

        public InvoiceService(IInvoiceRepository invoiceRepository, IPayOSService payOSService)
        {
            _invoiceRepository = invoiceRepository;
            _payOSService = payOSService;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAll()
        {
            return await _invoiceRepository.GetAllInvoices();
        }

        public async Task<string> GetPaymentUrl(int customerId, string invoiceId, UrlDto urlDto)
        {


            string timestamp = DateTime.Now.ToString();

            string url = await _payOSService.CreatePaymentLink(invoiceId, timestamp, 10000, urlDto.UrlCancel, urlDto.UrlReturn);

            return url;
        }

        public async Task PostTransactionProcess(int customerId, string invoiceId)
        {
            var transaction = await _payOSService.HandleCodeAfterPaymentQR(invoiceId);


        }
    }
}
