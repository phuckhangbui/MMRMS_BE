using Microsoft.Extensions.Logging;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class BackgroundServiceImpl : IBackgroundService
    {
        private readonly ILogger<IBackgroundService> _logger;
        private readonly IPromotionRepository _promotionRepository;

        public BackgroundServiceImpl(ILogger<IBackgroundService> logger, IPromotionRepository promotionRepository)
        {
            _logger = logger;
            _promotionRepository = promotionRepository;
        }

        public async Task PromotionJob()
        {
            try
            {
                _logger.LogInformation($"Starting promotion job");

                await _promotionRepository.UpdatePromotionToExpired();

                await _promotionRepository.UpdatePromotionToActive();

                _logger.LogInformation($"Promotion job execute successfully at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while update promotions to expire");
            }
        }
    }
}
