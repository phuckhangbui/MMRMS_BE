using DTOs.Promotion;

namespace Repository.Interface
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<PromotionDto>> GetPromotions();
        Task CreatePromotion(PromotionCreateRequestDto promotionCreateRequestDto);
        Task UpdatePromotionToExpired();
        Task UpdatePromotionToActive();
    }
}
