using DTOs.Promotion;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class PromotionServiceImpl : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionServiceImpl(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task CreatePromotion(PromotionCreateRequestDto promotionCreateRequestDto)
        {
            await _promotionRepository.CreatePromotion(promotionCreateRequestDto);
        }

        public async Task<IEnumerable<PromotionDto>> GetPromotions()
        {
            var promotions = await _promotionRepository.GetPromotions();

            if (promotions.IsNullOrEmpty())
            {
                throw new ServiceException("Promotion list is empty");
            }

            return promotions;
        }
    }
}
