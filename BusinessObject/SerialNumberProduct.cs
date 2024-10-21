﻿namespace BusinessObject;

public partial class SerialNumberProduct
{
    public string SerialNumber { get; set; } = null!;

    public int? ProductId { get; set; }

    public double? ActualRentPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreate { get; set; }

    public int? RentTimeCounter { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductComponentStatus> ProductComponentStatuses { get; set; } = new List<ProductComponentStatus>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public virtual ICollection<ComponentReplacementTicket> ComponentReplacementTickets { get; set; } = new List<ComponentReplacementTicket>();
    public virtual ICollection<SerialNumberProductLog> SerialNumberProductLogs { get; set; } = new List<SerialNumberProductLog>();

}
