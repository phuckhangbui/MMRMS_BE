using DTOs.ComponentReplacementTicket;
using DTOs.ContractPayment;

namespace DTOs.Invoice
{
    public class ContractInvoiceDto
    {
        public string InvoiceId { get; set; } = null!;
        public int? AccountPaidId { get; set; }
        public string? AccountPaidName { get; set; }
        public string? DigitalTransactionId { get; set; }
        public string? PaymentMethod { get; set; }
        public double? Amount { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DatePaid { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Note { get; set; }
        public string? RentingRequestId { get; set; }
        public List<ContractPaymentDto>? ContractPayments { get; set; }
        public FirstRentalPaymentDto? FirstRentalPayment { get; set; }
        public string? PaymentConfirmationUrl { get; set; }
        public List<ComponentReplacementTicketDto>? ComponentReplacementTickets { get; set; } = new List<ComponentReplacementTicketDto>();
        public double? RefundShippingPrice { get; set; }
    }
}
