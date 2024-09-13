namespace BusinessObject;

public partial class ComponentProduct
{
    public int ComponentProductId { get; set; }

    public string? ComponentName { get; set; }

    public int? Quantity { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ProductComponentStatus> ProductComponentStatuses { get; set; } = new List<ProductComponentStatus>();

    public virtual ICollection<ProductComponentDetail> ProductComponentDetails { get; set; } = new List<ProductComponentDetail>();
}
