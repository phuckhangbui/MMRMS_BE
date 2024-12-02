using DTOs.Invoice;

namespace Service.Interface
{
    public interface IPayOSService
    {
        Task<string> CreatePaymentLink(string invoiceId, int invoiceTimeStamp, int amount, string urlCancel, string urlReturn);
        Task<TransactionReturn> HandleCodeAfterPaymentQR(string code);
    }
}
