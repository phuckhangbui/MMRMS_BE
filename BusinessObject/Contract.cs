namespace BusinessObject;

public partial class Contract
{
    public string ContractId { get; set; } = null!;

    public string? ContractName { get; set; }

    public int? AccountSignId { get; set; }

    public string? RentingRequestId { get; set; }

    public double? RentPrice { get; set; }

    public double? DepositPrice { get; set; }

    public int? NumberOfMonth { get; set; }

    public int? RentPeriod { get; set; }

    public double? TotalRentPrice { get; set; }

    public string? Content { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateSign { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string? Status { get; set; }

    public string? SerialNumber { get; set; }

    public virtual MachineSerialNumber? ContractMachineSerialNumber { get; set; }

    public virtual Account? AccountSign { get; set; }

    public virtual RentingRequest? RentingRequest { get; set; }

    public virtual ICollection<ContractPayment> ContractPayments { get; set; } = new List<ContractPayment>();

    public virtual ICollection<ContractTerm> ContractTerms { get; set; } = new List<ContractTerm>();

    public virtual ICollection<DeliveryTask> Deliveries { get; set; } = new List<DeliveryTask>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<MachineCheckRequest> MachineCheckRequests { get; set; } = new List<MachineCheckRequest>();

    public virtual ICollection<ComponentReplacementTicket> ComponentReplacementTickets { get; set; } = new List<ComponentReplacementTicket>();

    public virtual ICollection<MachineTask> MachineTasks { get; set; } = new List<MachineTask>();
}
