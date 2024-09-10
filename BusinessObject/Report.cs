using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Report
{
    public int ReportId { get; set; }

    public int? TaskId { get; set; }

    public string? ReportContent { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual Task? Task { get; set; }
}
