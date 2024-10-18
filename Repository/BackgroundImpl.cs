using DAO;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public class BackgroundImpl : IBackground
    {
        private readonly ILogger<IBackground> _logger;
        private readonly RentingRequestDao _rentingRequestDao;

        public BackgroundImpl(ILogger<IBackground> logger, RentingRequestDao rentingRequestDao)
        {
            _logger = logger;
            _rentingRequestDao = rentingRequestDao;
        }

        public void ScheduleCancelRentingRequestJob(string rentingRequestId)
        {
            try
            {
                _logger.LogInformation($"Starting ScheduleCancelRentingRequestJob");

                TimeSpan delayToStart = TimeSpan.FromSeconds(10);

                BackgroundJob.Schedule(() => _rentingRequestDao.CancelRentingRequest(rentingRequestId), delayToStart);
                _logger.LogInformation($"Renting request: {rentingRequestId} scheduled for status change: " +
                    $"'Canceled' at {delayToStart}");

                _logger.LogInformation($"ScheduleCancelRentingRequestJob execute successfully at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while ScheduleCancelRentingRequestJob");
            }
        }
    }
}
