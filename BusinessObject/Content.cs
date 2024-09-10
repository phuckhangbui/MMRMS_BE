using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Content
{
    public int ContentId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Summary { get; set; }

    public string? Content1 { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public int? AccountCreateId { get; set; }

    public virtual Account? AccountCreate { get; set; }
}
