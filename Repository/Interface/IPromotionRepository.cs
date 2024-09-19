using DTOs.Promotion;

namespace Repository.Interface
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<PromotionDto>> GetPromotions();
        Task<PromotionDto?> GetPromotionById(int promotionId);
        Task CreatePromotion(PromotionRequestDto promotionRequestDto);
        Task UpdatePromotion(int promotionId, PromotionRequestDto promotionRequestDto);
        Task DeletePromotion(int promotionId);
        Task UpdatePromotionToExpired();
        Task UpdatePromotionToActive();
    }
}
