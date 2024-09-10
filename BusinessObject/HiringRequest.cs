using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class HiringRequest
{
    public int HiringRequestId { get; set; }

    public int? AccountOrderId { get; set; }

    public int? AddressId { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual Account? AccountOrder { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<HiringRequestProductDetail> HiringRequestProductDetails { get; set; } = new List<HiringRequestProductDetail>();
}
