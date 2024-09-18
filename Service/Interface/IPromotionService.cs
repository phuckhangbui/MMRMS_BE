using DTOs.Promotion;

namespace Service.Interface
{
    public interface IPromotionService
    {
        Task<IEnumerable<PromotionDto>> GetPromotions();
        Task CreatePromotion(PromotionCreateRequestDto promotionCreateRequestDto);
    }
}
