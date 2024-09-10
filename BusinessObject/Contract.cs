namespace BusinessObject;

public partial class Contract
{
    public int ContractId { get; set; }

    public string? ContractName { get; set; }

    public int? AccountSignId { get; set; }

    public int? AddressId { get; set; }

    public int? OrderId { get; set; }

    public double? Price { get; set; }

    public double? ShippingPrice { get; set; }

    public double? DiscountPrice { get; set; }

    public double? FinalPrice { get; set; }

    public int? Method { get; set; }

    public DateTime? DateSign { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string? Status { get; set; }

    public virtual Account? AccountSign { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<ContractPayment> ContractPayments { get; set; } = new List<ContractPayment>();

    public virtual ICollection<ContractTerm> ContractTerms { get; set; } = new List<ContractTerm>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual ICollection<SerialMechanicalMachinery> SerialMechanicalMachineries { get; set; } = new List<SerialMechanicalMachinery>();

    public virtual ICollection<EmployeeTask> EmployeeTasks { get; set; } = new List<EmployeeTask>();
}
