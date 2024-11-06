using Common.Enum;
using Hangfire;
using Microsoft.Extensions.Logging;
using Repository.Interface;
using Service.Interface;

namespace Service
{
    public class BackgroundImpl : IBackground
    {
        private readonly ILogger<BackgroundImpl> _logger;
        private readonly IRentingRequestRepository _rentingRequestRepository;
        private readonly IContractService _contractService;

        public BackgroundImpl(ILogger<BackgroundImpl> logger,
            IRentingRequestRepository rentingRequestRepository,
            IContractService contractService)
        {
            _logger = logger;
            _rentingRequestRepository = rentingRequestRepository;
            _contractService = contractService;
        }

        public void CancelRentingRequestJob(string rentingRequestId)
        {
            try
            {
                _logger.LogInformation($"Starting ScheduleCancelRentingRequestJob");

                TimeSpan delayToStart = TimeSpan.FromDays(1);

                BackgroundJob.Schedule(() => CancelRentingRequestAsync(rentingRequestId), delayToStart);
                _logger.LogInformation($"Renting request: {rentingRequestId} scheduled for status change: {RentingRequestStatusEnum.Canceled.ToString()} at {delayToStart}");

                //_logger.LogInformation($"ScheduleCancelRentingRequestJob execute successfully at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while ScheduleCancelRentingRequestJob");
            }
        }

        public void CompleteContractOnTimeJob(string contractId, TimeSpan delayToStart)
        {
            try
            {
                _logger.LogInformation($"Starting ScheduleCompleteContractOnTimeJob");

                BackgroundJob.Schedule(() => CompleteContractOnTimeAsync(contractId), delayToStart);
                _logger.LogInformation($"Contract: {contractId} scheduled for status change: {ContractStatusEnum.Completed.ToString()} at {delayToStart}");

                //_logger.LogInformation($"ScheduleCompleteContractOnTimeJob execute successfully at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while ScheduleCompleteContractOnTimeJob");
            }
        }

        public async Task CompleteContractOnTimeAsync(string contractId)
        {
            await CompleteContractOnTime(contractId);
        }
        public async Task CompleteContractOnTime(string contractId)
        {
            await _contractService.EndContract(contractId, null);
        }

        public async Task CancelRentingRequestAsync(string rentingRequestId)
        {
            await CancelRentingRequest(rentingRequestId);
        }
        public async Task CancelRentingRequest(string rentingRequestId)
        {
            var isValid = await _rentingRequestRepository.IsRentingRequestValidToCancel(rentingRequestId);
            if (isValid)
            {
                await _rentingRequestRepository.CancelRentingRequest(rentingRequestId);
            }
        }
    }
}
