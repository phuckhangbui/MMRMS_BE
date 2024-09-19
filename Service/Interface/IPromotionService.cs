using DTOs.Promotion;

namespace Service.Interface
{
    public interface IPromotionService
    {
        Task<IEnumerable<PromotionDto>> GetPromotions();
        Task<PromotionDto> GetPromotionById(int promotionId);
        Task CreatePromotion(PromotionRequestDto promotionCreateRequestDto);
        Task UpdatePromotion(int promotionId, PromotionRequestDto promotionRequestDto);
        Task DeletePromotion(int promotionId);
    }
}
