namespace BusinessObject;

public partial class ContractPayment
{
    public int ContractPaymentId { get; set; }

    public string? ContractId { get; set; }

    public string? InvoiceId { get; set; }

    public string? Title { get; set; }

    public double? Amount { get; set; }

    public DateTime? CustomerPaidDate { get; set; }

    public DateTime? SystemPaidDate { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DueDate { get; set; }

    public bool? IsFirstRentalPayment { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
