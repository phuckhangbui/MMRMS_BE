﻿namespace BusinessObject;

public partial class EmployeeTask
{
    public int EmployeeTaskId { get; set; }

    public string? TaskTitle { get; set; }

    public string? ContractId { get; set; }

    public string? Content { get; set; }

    public int? StaffId { get; set; }

    public int? ManagerId { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateCompleted { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public string? Type { get; set; }

    public virtual Account? Staff { get; set; }

    public virtual Account? Manager { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual ICollection<MaintainingTicket> MaintainingTickets { get; set; } = new List<MaintainingTicket>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();
}
