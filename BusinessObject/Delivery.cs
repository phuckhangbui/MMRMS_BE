using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public int? StaffId { get; set; }

    public int? ContractId { get; set; }

    public DateTime? DateShip { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual Account? Staff { get; set; }
}
