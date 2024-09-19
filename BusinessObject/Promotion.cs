namespace BusinessObject;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string? DiscountTypeName { get; set; }

    public double? DiscountPercentage { get; set; }

    public string? Content { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<AccountPromotion> AccountPromotions { get; set; } = new List<AccountPromotion>();

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

}
