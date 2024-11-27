namespace BusinessObject;

public partial class Contract
{
    public string ContractId { get; set; } = null!;

    public string? ContractName { get; set; }

    public int? AccountSignId { get; set; }

    public string? RentingRequestId { get; set; }

    public double? RentPrice { get; set; }

    public double? DepositPrice { get; set; }

    public int? RentPeriod { get; set; }

    public double? TotalRentPrice { get; set; }

    public string? Content { get; set; }

    public double? ShippingDistance { get; set; }

    public double? ShippingPrice { get; set; }

    public double? RefundShippingPrice { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateSign { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string? Status { get; set; }

    public string? SerialNumber { get; set; }

    public string? BaseContractId { get; set; }

    public bool? IsExtended { get; set; }

    public virtual MachineSerialNumber? ContractMachineSerialNumber { get; set; }

    public virtual Account? AccountSign { get; set; }

    public virtual RentingRequest? RentingRequest { get; set; }

    public virtual Contract? BaseContract { get; set; }

    public virtual ICollection<ContractPayment> ContractPayments { get; set; } = new List<ContractPayment>();

    public virtual ICollection<ContractTerm> ContractTerms { get; set; } = new List<ContractTerm>();

    public virtual ICollection<ContractDelivery> ContractDeliveries { get; set; } = new List<ContractDelivery>();

    public virtual ICollection<MachineCheckRequest> MachineCheckRequests { get; set; } = new List<MachineCheckRequest>();

    public virtual ICollection<ComponentReplacementTicket> ComponentReplacementTickets { get; set; } = new List<ComponentReplacementTicket>();

    public virtual ICollection<MachineTask> MachineTasks { get; set; } = new List<MachineTask>();
}
