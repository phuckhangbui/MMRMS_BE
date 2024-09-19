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

        public async Task CreatePromotion(PromotionRequestDto promotionCreateRequestDto)
        {
            await _promotionRepository.CreatePromotion(promotionCreateRequestDto);
        }

        public async Task DeletePromotion(int promotionId)
        {
            await CheckPrmotionExist(promotionId);

            await _promotionRepository.DeletePromotion(promotionId);
        }

        public async Task<PromotionDto> GetPromotionById(int promotionId)
        {
            var promotion = await CheckPrmotionExist(promotionId);

            return promotion;
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

        public async Task UpdatePromotion(int promotionId, PromotionRequestDto promotionRequestDto)
        {
            await CheckPrmotionExist(promotionId);

            await _promotionRepository.UpdatePromotion(promotionId, promotionRequestDto);
        }

        private async Task<PromotionDto> CheckPrmotionExist(int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionById(promotionId);

            if (promotion == null)
            {
                throw new ServiceException("Promotion not found");
            }

            return promotion;
        }
    }
}
