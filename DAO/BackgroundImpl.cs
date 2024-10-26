using Common.Enum;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace DAO
{
    public class BackgroundImpl : IBackground
    {
        private readonly ILogger<BackgroundImpl> _logger;
        private readonly RentingRequestDao _rentingRequestDao;
        private readonly ContractDao _contractDao;

        public BackgroundImpl(ILogger<BackgroundImpl> logger, RentingRequestDao rentingRequestDao, ContractDao contractDao)
        {
            _logger = logger;
            _rentingRequestDao = rentingRequestDao;
            _contractDao = contractDao;
        }

        public void ScheduleCancelRentingRequestJob(string rentingRequestId)
        {
            try
            {
                _logger.LogInformation($"Starting ScheduleCancelRentingRequestJob");

                TimeSpan delayToStart = TimeSpan.FromSeconds(10);

                BackgroundJob.Schedule(() => _rentingRequestDao.CancelRentingRequest(rentingRequestId), delayToStart);
                _logger.LogInformation($"Renting request: {rentingRequestId} scheduled for status change: {RentingRequestStatusEnum.Canceled.ToString()} at {delayToStart}");

                //_logger.LogInformation($"ScheduleCancelRentingRequestJob execute successfully at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while ScheduleCancelRentingRequestJob");
            }
        }

        public void ScheduleCompleteContractOnTimeJob(string contractId, TimeSpan delayToStart)
        {
            try
            {
                _logger.LogInformation($"Starting ScheduleCompleteContractOnTimeJob");

                BackgroundJob.Schedule(() => _contractDao.CompleteContractOnTime(contractId), delayToStart);
                _logger.LogInformation($"Contract: {contractId} scheduled for status change: {ContractStatusEnum.Completed.ToString()} at {delayToStart}");

                //_logger.LogInformation($"ScheduleCompleteContractOnTimeJob execute successfully at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while ScheduleCompleteContractOnTimeJob");
            }
        }
    }
}
