﻿namespace BusinessObject;

public partial class TaskLog
{
    public int TaskLogId { get; set; }

    public int? EmployeeTaskId { get; set; }

    public string? Action { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual EmployeeTask? EmployeeTask { get; set; }
}
