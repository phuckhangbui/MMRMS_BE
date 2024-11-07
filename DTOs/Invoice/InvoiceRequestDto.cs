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
        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.Invoice.InvoiceAmountPositive)]
        public double? Amount { get; set; }
    }
}
