using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class PromotionType
{
    public int PromotionTypeId { get; set; }

    public string? PromotionTypeName { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
