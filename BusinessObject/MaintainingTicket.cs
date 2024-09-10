using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class MaintainingTicket
{
    public int MaintainingTicketId { get; set; }

    public string? SerialNumber { get; set; }

    public int? TaskId { get; set; }

    public int? ComponentId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? DateRepair { get; set; }

    public int? Type { get; set; }

    public string? Note { get; set; }

    public virtual Component? Component { get; set; }

    public virtual Task? Task { get; set; }
}
