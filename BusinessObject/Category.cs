namespace BusinessObject;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
