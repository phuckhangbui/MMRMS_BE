using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Invoice
{
    public class InvoiceRequestDto
    {

    }

    public class RefundInvoiceRequestDto
    {
        [Required(ErrorMessage = MessageConstant.Invoice.ContractIdRequired)]
        public string? ContractId { get; set; }

        [Required(ErrorMessage = MessageConstant.Invoice.InvoiceAmountRequired)]
        public double? Amount { get; set; }

        public string? PaymentConfirmationUrl { get; set; }
    }
}
