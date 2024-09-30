using DTOs.Promotion;

namespace DTOs.AccountPromotion
{
    public class AccountPromotionDto
    {
        public int AccountPromotionId { get; set; }
        public int? PromotionId { get; set; }
        public int? AccountId { get; set; }
        public DateTime? DateReceive { get; set; }
        public int? Status { get; set; }
        public PromotionDto Promotion { get; set; }
    }
}
