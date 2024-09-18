﻿namespace BusinessObject;

public partial class Component
{
    public int ComponentId { get; set; }

    public string? ComponentName { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }


    public virtual ICollection<MaintainingTicket> MaintainingTickets { get; set; } = new List<MaintainingTicket>();
    public virtual ICollection<ComponentProduct> ComponentProducts { get; set; } = new List<ComponentProduct>();

}
