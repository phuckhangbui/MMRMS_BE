namespace BusinessObject;

public partial class MachineSerialNumberComponent
{
    public int MachineSerialNumberComponentId { get; set; }

    public string? SerialNumber { get; set; }

    public int? MachineComponentId { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual MachineComponent? MachineComponent { get; set; }

    public virtual MachineSerialNumber? MachineSerialNumber { get; set; }

    public virtual ICollection<MachineSerialNumberLog> MachineSerialNumberLogs { get; set; } = new List<MachineSerialNumberLog>();

    public virtual ICollection<ComponentReplacementTicket> ComponentReplacementTickets { get; set; } = new List<ComponentReplacementTicket>();


}
