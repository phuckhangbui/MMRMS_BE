namespace BusinessObject;

public partial class ProductAttribute
{
    public int ProductAttributeId { get; set; }

    public int? ProductId { get; set; }

    public string? AttributeName { get; set; }

    public string? Specifications { get; set; }

    public string? Unit { get; set; }

    public virtual Product? Product { get; set; }
}
