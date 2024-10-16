namespace BusinessObject;

public partial class Invoice
{
    public string InvoiceId { get; set; } = null!;

    public string? InvoiceCode { get; set; }

    public int? AccountPaidId { get; set; }

    public string? MaintainTicketId { get; set; }

    public string? DigitalTransactionId { get; set; }

    public string? PaymentMethod { get; set; }

    public double? Amount { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<ContractPayment> ContractPayments { get; set; } = new List<ContractPayment>();

    public string? PayOsOrderId { get; set; }

    public virtual Account? AccountPaid { get; set; }

    public virtual MaintenanceTicket? MaintenanceTicket { get; set; }

    public virtual DigitalTransaction? DigitalTransaction { get; set; }

}
