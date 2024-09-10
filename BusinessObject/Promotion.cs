using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string? PromotionPack { get; set; }

    public int? ActionPromotion { get; set; }

    public double? DiscountPercentage { get; set; }

    public int? PromotionTypeId { get; set; }

    public int? DiscountTypeId { get; set; }

    public string? Content { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<AccountPromotion> AccountPromotions { get; set; } = new List<AccountPromotion>();

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual DiscountType? DiscountType { get; set; }

    public virtual PromotionType? PromotionType { get; set; }
}
