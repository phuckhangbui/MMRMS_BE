using DTOs.AccountPromotion;
using DTOs.MembershipRank;

namespace DTOs.RentingRequest
{
    public class RentingRequestInitDataDto
    {
        public AccountPromotionDto AccountPromotionDto { get; set; }
        public MembershipRankDto MembershipRankDto { get; set; }
    }
}
