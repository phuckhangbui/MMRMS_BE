namespace BusinessObject;

public partial class ProductComponentStatus
{
    public int ProductComponentStatusId { get; set; }

    public string? SerialNumber { get; set; }

    public int? ComponentId { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual ComponentProduct? Component { get; set; }

    public virtual SerialNumberProduct? SerialNumberProduct { get; set; }

}
