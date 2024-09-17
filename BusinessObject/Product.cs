namespace BusinessObject;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public string? Model { get; set; }

    public string? Origin { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<HiringRequestProductDetail> HiringRequestProductDetails { get; set; } = new List<HiringRequestProductDetail>();


    public virtual ICollection<SerialNumberProduct> SerialNumberProducts { get; set; } = new List<SerialNumberProduct>();
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public virtual ICollection<ComponentProduct> ComponentProducts { get; set; } = new List<ComponentProduct>();



}
