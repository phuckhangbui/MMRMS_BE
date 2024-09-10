using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class ProductNumber
{
    public string SerialNumber { get; set; } = null!;

    public int? ProductId { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductComponentStatus> ProductComponentStatuses { get; set; } = new List<ProductComponentStatus>();

    public virtual ICollection<SerialMechanicalMachinery> SerialMechanicalMachineries { get; set; } = new List<SerialMechanicalMachinery>();
}
