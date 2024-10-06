namespace BusinessObject;

public partial class LogDetail
{
    public int LogDetailId { get; set; }

    public int? AccountId { get; set; }

    public string? Action { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual Account? Account { get; set; }
}
