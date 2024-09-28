namespace BusinessObject;

public partial class Invoice
{
    public string InvoiceId { get; set; } = null!;

    public string? InvoiceCode { get; set; }

    public int? AccountPaidId { get; set; }

    public int? ContractPaymentId { get; set; }

    public string? MaintainTicketId { get; set; }

    public string? PaymentMethod { get; set; }

    public double? Amount { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public string? Note { get; set; }

    public virtual ContractPayment? ContractPayment { get; set; }

    public virtual Account? AccountPaid { get; set; }

    public virtual MaintainingTicket? MaintainingTicket { get; set; }

}
