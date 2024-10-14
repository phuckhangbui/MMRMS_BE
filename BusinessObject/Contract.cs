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

    public double? TotalRentPrice { get; set; }

    public string? Content { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateSign { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string? Status { get; set; }

    public string? SerialNumber { get; set; }

    public virtual SerialNumberProduct? ContractSerialNumberProduct { get; set; }

    public virtual Account? AccountSign { get; set; }

    public virtual RentingRequest? RentingRequest { get; set; }

    public virtual ICollection<ContractPayment> ContractPayments { get; set; } = new List<ContractPayment>();

    public virtual ICollection<ContractTerm> ContractTerms { get; set; } = new List<ContractTerm>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual ICollection<MaintenanceTicket> MaintenanceTickets { get; set; } = new List<MaintenanceTicket>();

    public virtual ICollection<EmployeeTask> EmployeeTasks { get; set; } = new List<EmployeeTask>();
}
