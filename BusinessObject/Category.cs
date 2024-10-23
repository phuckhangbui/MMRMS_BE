namespace BusinessObject;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
}
