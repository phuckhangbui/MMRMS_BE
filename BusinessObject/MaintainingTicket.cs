namespace BusinessObject;

public partial class MaintainingTicket
{
    public int MaintainingTicketId { get; set; }

    public string? SerialNumber { get; set; }

    public int? EmployeeTaskId { get; set; }

    public int? ComponentId { get; set; }

    public string? ProductSerialNumber { get; set; }

    public int? Quantity { get; set; }

    public DateTime? DateRepair { get; set; }

    public int? Type { get; set; }

    public string? Note { get; set; }

    public virtual Component? Component { get; set; }

    public virtual EmployeeTask? EmployeeTask { get; set; }

    public virtual SerialNumberProduct? SerialNumberProduct { get; set; }

}
