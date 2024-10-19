namespace BusinessObject;

public partial class Content
{
    public int ContentId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Summary { get; set; }

    public string? ContentBody { get; set; }

    public string? Title { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
