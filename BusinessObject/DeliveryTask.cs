namespace BusinessObject;

public partial class DeliveryTask
{
    public int DeliveryTaskId { get; set; }

    public int? StaffId { get; set; }

    public int? ManagerId { get; set; }

    public DateTime? DateShip { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateCompleted { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public string? ConfirmationPictureUrl { get; set; }

    public string? ReceiverName { get; set; }

    public string? Type { get; set; }

    public virtual Account? Staff { get; set; }

    public virtual Account? Manager { get; set; }

    public virtual ICollection<ContractDelivery> ContractDeliveries { get; set; } = new List<ContractDelivery>();

    public virtual ICollection<DeliveryTaskLog> DeliveryTaskLogs { get; set; } = new List<DeliveryTaskLog>();

}
