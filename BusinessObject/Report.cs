namespace BusinessObject;

public partial class Report
{
    public int ReportId { get; set; }

    public int? EmployeeTaskId { get; set; }

    public string? ReportContent { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual EmployeeTask? EmployeeTask { get; set; }
}
