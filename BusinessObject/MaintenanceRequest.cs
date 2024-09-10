using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class MaintenanceRequest
{
    public int RequestId { get; set; }

    public int? ContractId { get; set; }

    public string? SerialNumber { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual ICollection<RequestDateResponse> RequestDateResponses { get; set; } = new List<RequestDateResponse>();

    public virtual ProductNumber? SerialNumberNavigation { get; set; }
}
