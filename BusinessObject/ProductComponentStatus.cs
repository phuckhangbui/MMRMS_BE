using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class ProductComponentStatus
{
    public int ProductComponentStatusId { get; set; }

    public string? SerialNumber { get; set; }

    public int? ComponentId { get; set; }

    public int? Quantity { get; set; }

    public int? Status { get; set; }

    public virtual ComponentProduct? Component { get; set; }

    public virtual ProductNumber? SerialNumberNavigation { get; set; }
}
