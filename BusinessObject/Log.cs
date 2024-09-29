namespace BusinessObject;

public partial class Log
{
    public int LogId { get; set; }

    public int? AccountLogId { get; set; }

    public DateTime? DateUpdate { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual Account? AccountLog { get; set; }

    public virtual ICollection<LogDetail> LogDetails { get; set; } = new List<LogDetail>();
}
