namespace BusinessObject;

public partial class MachineComponent
{
    public int MachineComponentId { get; set; }

    public int? MachineId { get; set; }

    public int? ComponentId { get; set; }

    public int? Quantity { get; set; }

    public Component? Component { get; set; }

    public Machine? Machine { get; set; }

    public virtual ICollection<MachineSerialNumberComponent> MachineSerialNumberComponents { get; set; } = new List<MachineSerialNumberComponent>();

}
