namespace BusinessObject;

public partial class ComponentProduct
{
    public int ComponentProductId { get; set; }

    public int? ProductId { get; set; }

    public int? ComponentId { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public bool? IsRequiredMoney { get; set; }

    public Component? Component { get; set; }
    public Product? Product { get; set; }

    public virtual ICollection<ProductComponentStatus> ProductComponentStatuses { get; set; } = new List<ProductComponentStatus>();

}
