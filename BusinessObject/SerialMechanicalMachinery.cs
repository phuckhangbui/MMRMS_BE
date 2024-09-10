using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class SerialMechanicalMachinery
{
    public int ContractOrderId { get; set; }

    public int? ContractId { get; set; }

    public string? SerialNumber { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual ProductNumber? SerialNumberNavigation { get; set; }
}
