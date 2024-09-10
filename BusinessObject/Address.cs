using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Address
{
    public int AddressId { get; set; }

    public int? AccountId { get; set; }

    public string? Address1 { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<HiringRequest> HiringRequests { get; set; } = new List<HiringRequest>();
}
