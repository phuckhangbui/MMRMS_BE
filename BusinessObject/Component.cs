using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Component
{
    public int ComponentId { get; set; }

    public string? ComponentName { get; set; }

    public int? Quantity { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<MaintainingTicket> MaintainingTickets { get; set; } = new List<MaintainingTicket>();
}
