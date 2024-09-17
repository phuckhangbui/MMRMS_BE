namespace BusinessObject;

public partial class Invoice
{
    public string InvoiceId { get; set; } = null!;

    public string? InvoiceCode { get; set; }

    public string? ContractId { get; set; }

    public int? Method { get; set; }

    public double? Price { get; set; }

    public DateTime? DateCreate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Contract? Contract { get; set; }
}
