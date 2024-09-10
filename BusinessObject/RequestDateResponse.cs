using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class RequestDateResponse
{
    public int ResponseDateId { get; set; }

    public int? RequestId { get; set; }

    public DateTime? DateResponse { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual MaintenanceRequest? Request { get; set; }
}
