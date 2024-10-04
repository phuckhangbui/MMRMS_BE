namespace BusinessObject;

public partial class SerialNumberProduct
{
    public string SerialNumber { get; set; } = null!;

    public int? ProductId { get; set; }

    public double? ActualRentPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreate { get; set; }

    public int? RentTimeCounter { get; set; }

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductComponentStatus> ProductComponentStatuses { get; set; } = new List<ProductComponentStatus>();

    public virtual ICollection<ContractSerialNumberProduct> ContractSerialNumberProducts { get; set; } = new List<ContractSerialNumberProduct>();
    public virtual ICollection<MaintenanceTicket> MaintenanceTickets { get; set; } = new List<MaintenanceTicket>();

}
