namespace BusinessObject;

public partial class RequestResponse
{
    public int RequestResponseId { get; set; }

    public string? RequestId { get; set; }

    public int? EmployeeTaskId { get; set; }

    public DateTime? DateResponse { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Action { get; set; }

    public virtual MaintenanceRequest? Request { get; set; }

    public virtual EmployeeTask? EmployeeTask { get; set; }
}
