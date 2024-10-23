namespace BusinessObject;

public partial class MachineAttribute
{
    public int MachineAttributeId { get; set; }

    public int? MachineId { get; set; }

    public string? AttributeName { get; set; }

    public string? Specifications { get; set; }

    public string? Unit { get; set; }

    public virtual Machine? Machine { get; set; }
}
