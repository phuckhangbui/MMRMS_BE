namespace BusinessObject;

public partial class ComponentReplacementTicket
{
    public string ComponentReplacementTicketId { get; set; }

    public int? EmployeeCreateId { get; set; }

    public int? MachineTaskCreateId { get; set; }

    public string? ContractId { get; set; }

    public int? ComponentId { get; set; }

    public string? InvoiceId { get; set; }

    public int? MachineSerialNumberComponentId { get; set; }

    public double? ComponentPrice { get; set; }

    public double? AdditionalFee { get; set; }

    public double? TotalAmount { get; set; }

    public int? Quantity { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateRepair { get; set; }

    public int? Type { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public virtual Component? Component { get; set; }

    public virtual ICollection<MachineTask>? MachineTasks { get; set; } = new List<MachineTask>();

    public virtual ICollection<ComponentReplacementTicketLog>? ComponentReplacementTicketLogs { get; set; } = new List<ComponentReplacementTicketLog>();

    public virtual MachineTask? MachineTaskCreate { get; set; }

    public virtual Account? EmployeeCreate { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual MachineSerialNumberComponent? MachineSerialNumberComponent { get; set; }


}
