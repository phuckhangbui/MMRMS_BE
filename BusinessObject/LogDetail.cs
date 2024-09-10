using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class LogDetail
{
    public int LogDetailId { get; set; }

    public int? LogId { get; set; }

    public string? Action { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual Log? Log { get; set; }
}
