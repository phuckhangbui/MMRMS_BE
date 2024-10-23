namespace BusinessObject;

public partial class MachineSerialNumber
{
    public string SerialNumber { get; set; } = null!;

    public int? MachineId { get; set; }

    public double? ActualRentPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreate { get; set; }

    public int? RentDaysCounter { get; set; }

    public virtual Machine? Machine { get; set; }

    public virtual ICollection<MachineComponentStatus> MachineComponentStatuses { get; set; } = new List<MachineComponentStatus>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public virtual ICollection<ComponentReplacementTicket> ComponentReplacementTickets { get; set; } = new List<ComponentReplacementTicket>();
    public virtual ICollection<MachineSerialNumberLog> MachineSerialNumberLogs { get; set; } = new List<MachineSerialNumberLog>();

}
