using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class TaskLog
{
    public int TaskLogId { get; set; }

    public int? TaskId { get; set; }

    public string? Action { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual Task? Task { get; set; }
}
