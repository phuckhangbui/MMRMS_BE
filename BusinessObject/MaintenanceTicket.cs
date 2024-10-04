namespace BusinessObject;

public partial class MaintenanceTicket
{
    public int MaintenanceTicketId { get; set; }

    public int? EmployeeTaskId { get; set; }

    public int? EmployeeCreateId { get; set; }

    public string? ContractId { get; set; }

    public int? ComponentId { get; set; }

    public string? InvoiceId { get; set; }

    public string? ProductSerialNumber { get; set; }

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

    public virtual EmployeeTask? EmployeeTask { get; set; }

    public virtual Account? EmployeeCreate { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual SerialNumberProduct? SerialNumberProduct { get; set; }

}
