using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class DiscountType
{
    public int DiscountTypeId { get; set; }

    public string? DiscountTypeName { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
