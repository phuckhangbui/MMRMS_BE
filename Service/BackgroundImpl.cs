using Common.Enum;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Interface;

namespace Service
{
    public class BackgroundImpl : IBackground
    {
        private readonly ILogger<BackgroundImpl> _logger;
        private readonly IRentingRequestRepository _rentingRequestRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IContractService _contractService;

        public BackgroundImpl(ILogger<BackgroundImpl> logger,
            IContractService contractService,
            IRentingRequestRepository rentingRequestRepository,
            IContractRepository contractRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository)
        {
            _logger = logger;
            _contractService = contractService;
            _rentingRequestRepository = rentingRequestRepository;
            _contractRepository = contractRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
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
            await _contractService.EndContract(contractId);
        }

        public async Task CancelRentingRequestAsync(string rentingRequestId)
        {
            await CancelRentingRequest(rentingRequestId);
        }
        public async Task CancelRentingRequest(string rentingRequestId)
        {
            var contracts = await _contractRepository.GetContractDetailListByRentingRequestId(rentingRequestId);
            if (contracts.IsNullOrEmpty())
            {
                return;
            }

            var canCancel = true;
            foreach (var contract in contracts)
            {
                var isPaid = contract.ContractPayments
                    .Any(c => c.Status.Equals(ContractPaymentStatusEnum.Paid.ToString()));

                if (isPaid)
                {
                    canCancel = false;
                    break;
                }
            }

            if (canCancel)
            {
                foreach (var contract in contracts)
                {
                    await _machineSerialNumberRepository.UpdateStatus(
                        contract.SerialNumber,
                        MachineSerialNumberStatusEnum.Available.ToString(),
                        (int)contract.AccountSignId
                    );
                }

                await _rentingRequestRepository.CancelRentingRequest(rentingRequestId);
            }
        }
    }
}
