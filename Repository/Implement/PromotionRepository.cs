using AutoMapper;
using BusinessObject;
using DAO;
using Common.Enum;
using DTOs.Promotion;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IPromotionRepository> _logger;

        public PromotionRepository(IMapper mapper, ILogger<IPromotionRepository> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreatePromotion(PromotionRequestDto promotionRequestDto)
        {
            var promotion = _mapper.Map<Promotion>(promotionRequestDto);

            promotion.DateCreate = DateTime.Now;
            promotion.DateStart = promotionRequestDto.DateStart?.Date;
            promotion.DateEnd = promotionRequestDto.DateEnd?.Date.AddDays(1).AddSeconds(-1);

            if (promotion.DateStart > DateTime.Now)
            {
                promotion.Status = PromotionStatusEnum.Upcoming.ToString();
            }
            else
            {
                promotion.Status = PromotionStatusEnum.Active.ToString();
            }

            await PromotionDao.Instance.CreateAsync(promotion);
        }

        public async Task DeletePromotion(int promotionId)
        {
            var promotion = await PromotionDao.Instance.GetPromotionById(promotionId);
            if (promotion != null)
            {
                await PromotionDao.Instance.RemoveAsync(promotion);
            }
        }

        public async Task<PromotionDto?> GetPromotionById(int promotionId)
        {
            var promotion = await PromotionDao.Instance.GetPromotionById(promotionId);
            if (promotion != null)
            {
                return _mapper.Map<PromotionDto>(promotion);
            }

            return null;
        }

        public async Task<IEnumerable<PromotionDto>> GetPromotions()
        {
            var promotions = await PromotionDao.Instance.GetAllAsync();

            if (!promotions.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<PromotionDto>>(promotions);
            }

            return [];
        }

        public async Task UpdatePromotion(int promotionId, PromotionRequestDto promotionRequestDto)
        {
            var promotion = await PromotionDao.Instance.GetPromotionById(promotionId);
            if (promotion != null)
            {
                promotion = _mapper.Map<Promotion>(promotionRequestDto);
                promotion.PromotionId = promotionId;

                promotion.DateCreate = DateTime.Now;
                promotion.DateStart = promotionRequestDto.DateStart?.Date;
                promotion.DateEnd = promotionRequestDto.DateEnd?.Date.AddDays(1).AddSeconds(-1);

                if (promotion.DateStart > DateTime.Now)
                {
                    promotion.Status = PromotionStatusEnum.Upcoming.ToString();
                }
                else
                {
                    promotion.Status = PromotionStatusEnum.Active.ToString();
                }

                await PromotionDao.Instance.UpdateAsync(promotion);
            }
        }

        public async Task UpdatePromotionToActive()
        {
            var promotions = await PromotionDao.Instance.GetPromotionsForUpdateToActive();

            _logger.LogInformation($"Promotions change to active: {promotions.Count()}");

            if (!promotions.IsNullOrEmpty())
            {
                foreach (var promotion in promotions)
                {
                    if (promotion.Status == PromotionStatusEnum.Upcoming.ToString())
                    {
                        promotion.Status = PromotionStatusEnum.Active.ToString();

                        await PromotionDao.Instance.UpdateAsync(promotion);
                    }
                }

                _logger.LogInformation("End UpdatePromotionToActive");
            }
        }

        public async Task UpdatePromotionToExpired()
        {
            var promotions = await PromotionDao.Instance.GetPromotionsForUpdateToExpired();

            _logger.LogInformation($"Promotions change to expired: {promotions.Count()}");

            if (!promotions.IsNullOrEmpty())
            {
                foreach (var promotion in promotions)
                {
                    if (promotion.Status == PromotionStatusEnum.Active.ToString())
                    {
                        promotion.Status = PromotionStatusEnum.Expired.ToString();

                        await PromotionDao.Instance.UpdateAsync(promotion);
                    }
                }

                _logger.LogInformation("End UpdatePromotionToExpired");
            }
        }
    }
}
