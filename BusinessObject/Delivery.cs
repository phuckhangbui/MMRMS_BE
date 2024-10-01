namespace BusinessObject;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public int? StaffId { get; set; }

    public string? ContractId { get; set; }

    public DateTime? DateShip { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateCompleted { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual Account? Staff { get; set; }

    public virtual ICollection<DeliveryLog> DeliveryLogs { get; set; } = new List<DeliveryLog>();
}
