namespace BusinessObject;

public partial class Component
{
    public int ComponentId { get; set; }

    public string? ComponentName { get; set; }

    public int? AvailableQuantity { get; set; }

    public double? Price { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public int? QuantityOnHold { get; set; }

    public virtual ICollection<ComponentReplacementTicket> ComponentReplacementTickets { get; set; } = new List<ComponentReplacementTicket>();
    public virtual ICollection<MachineComponent> MachineComponents { get; set; } = new List<MachineComponent>();

}
