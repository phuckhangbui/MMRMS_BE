namespace BusinessObject;

public partial class Machine
{
    public int MachineId { get; set; }

    public string? MachineName { get; set; }

    public double? RentPrice { get; set; }

    public double? Weight { get; set; }

    public double? MachinePrice { get; set; }

    public string? Model { get; set; }

    public string? Origin { get; set; }

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<MachineAttribute> MachineAttributes { get; set; } = new List<MachineAttribute>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<MachineSerialNumber> MachineSerialNumbers { get; set; } = new List<MachineSerialNumber>();
    public virtual ICollection<MachineImage> MachineImages { get; set; } = new List<MachineImage>();
    public virtual ICollection<MachineComponent> MachineComponents { get; set; } = new List<MachineComponent>();
    public virtual ICollection<MachineTerm> MachineTerms { get; set; } = new List<MachineTerm>();


}
