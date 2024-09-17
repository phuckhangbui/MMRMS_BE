namespace BusinessObject;

public partial class EmployeeTask
{
    public int EmployeeTaskId { get; set; }

    public string? TaskTitle { get; set; }

    public string? ContractId { get; set; }

    public string? Content { get; set; }

    public int? AssigneeId { get; set; }

    public int? ReporterId { get; set; }

    public DateTime? Deadline { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public int? TaskType { get; set; }

    public virtual Account? Assignee { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual ICollection<MaintainingTicket> MaintainingTickets { get; set; } = new List<MaintainingTicket>();

    public virtual Account? Reporter { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();
}
