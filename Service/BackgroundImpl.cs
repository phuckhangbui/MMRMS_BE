using Common.Enum;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Transactions;

namespace Service
{
    public class BackgroundImpl : IBackground
    {
        private readonly ILogger<BackgroundImpl> _logger;
        private readonly IRentingRequestRepository _rentingRequestRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;

        public BackgroundImpl(ILogger<BackgroundImpl> logger,
            IRentingRequestRepository rentingRequestRepository,
            IContractRepository contractRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IInvoiceRepository invoiceRepository,
            IMachineRepository machineRepository,
            ISettingsService settingsService,
            INotificationService notificationService)
        {
            _logger = logger;
            _rentingRequestRepository = rentingRequestRepository;
            _contractRepository = contractRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _invoiceRepository = invoiceRepository;
            _machineRepository = machineRepository;
            _settingsService = settingsService;
            _notificationService = notificationService;
        }

        public void CancelRentingRequestJob(string rentingRequestId)
        {
            try
            {
                _logger.LogInformation($"Starting ScheduleCancelRentingRequestJob for RentingRequestId: {rentingRequestId}.");

                TimeSpan delayToStart = TimeSpan.FromDays(1);

                BackgroundJob.Schedule(() => CancelRentingRequestAsync(rentingRequestId), delayToStart);
                _logger.LogInformation($"RentingRequestId: {rentingRequestId} scheduled for status change to '{RentingRequestStatusEnum.Canceled}' after a delay of {delayToStart.TotalHours} hours. " +
                    $"The job will execute at {DateTime.Now.Add(delayToStart):yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while scheduling ScheduleCancelRentingRequestJob for RentingRequestId: {rentingRequestId}");
            }
        }

        public void CompleteContractOnTimeJob(string contractId, TimeSpan delayToStart)
        {
            try
            {
                _logger.LogInformation($"Starting ScheduleCompleteContractOnTimeJob for ContractId: {contractId}.");

                BackgroundJob.Schedule(() => CompleteContractOnTimeAsync(contractId), delayToStart);
                _logger.LogInformation($"ContractId: {contractId} scheduled for status change to '{ContractStatusEnum.Completed}' after a delay of {delayToStart.TotalHours} hours. " +
                    $"The job will execute at {DateTime.Now.Add(delayToStart):yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while scheduling ScheduleCompleteContractOnTimeJob for ContractId: {contractId}");
            }
        }

        public void ProcessExtendContractJob(string contractId, TimeSpan delayToStart)
        {
            try
            {
                _logger.LogInformation($"Starting ProcessExtendContractJob for ContractId: {contractId}.");

                BackgroundJob.Schedule(() => ProcessExtendContracAsync(contractId), delayToStart);
                _logger.LogInformation($"ContractId: {contractId} scheduled for extension processing. " +
                    $"The job will execute after a delay of {delayToStart.TotalHours} hours, at {DateTime.Now.Add(delayToStart):yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while scheduling ProcessExtendContractJob for ContractId: {contractId}");
            }
        }

        public void ProcessOverdueContractPaymentJob(int contractPaymentId, TimeSpan delayToStart)
        {
            try
            {
                _logger.LogInformation($"Starting ProcessOverdueContractPaymentJob for ContractPaymentId: {contractPaymentId} with a delay of {delayToStart.TotalMinutes} minutes.");

                BackgroundJob.Schedule(() => ProcessOverdueContractPaymentAsync(contractPaymentId), delayToStart);

                _logger.LogInformation($"ContractPaymentId: {contractPaymentId} scheduled for overdue payment check. Job will process after {delayToStart.TotalMinutes} minutes at {DateTime.Now.Add(delayToStart):yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while scheduling ProcessOverdueContractPaymentJob for ContractPaymentId: {contractPaymentId}");
            }
        }

        public async Task ProcessOverdueContractPaymentAsync(int contractPaymentId)
        {
            await ProcessOverdueContractPayment(contractPaymentId);
        }
        public async Task ProcessOverdueContractPayment(int contractPaymentId)
        {
            var contractPayment = await _contractRepository.GetContractPayment(contractPaymentId);
            if (contractPayment != null && contractPayment.Status.Equals(ContractPaymentStatusEnum.Pending.ToString()))
            {
                //await CompleteContractOnTime(contractPayment.ContractId);
                var contract = await _contractRepository.GetContractById(contractPayment.ContractId);
                if (contract != null)
                {
                    _notificationService.SendNotificationToCustomerWhenLatePayment((int)contract.AccountSignId, contract.ContractId, (DateTime)contractPayment.DateFrom, contract.ContractId);
                }
            }
        }

        public async Task ProcessExtendContracAsync(string contractId)
        {
            await ProcessExtendContract(contractId);
        }
        public async Task ProcessExtendContract(string contractId)
        {
            var extendContract = await _contractRepository.GetContractDetailById(contractId);
            if (extendContract != null && extendContract.BaseContractId != null)
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var baseContract = await _contractRepository.GetContractById(extendContract.BaseContractId);

                    if (extendContract.Status.Equals(ContractStatusEnum.NotSigned.ToString()))
                    {
                        //Base contract
                        await CompleteContractOnTime(extendContract.BaseContractId);

                        //Extend contract
                        await _contractRepository.CancelExtendContractWhenNotSigned(extendContract.ContractId);
                    }

                    if (extendContract.Status.Equals(ContractStatusEnum.Signed.ToString()))
                    {
                        if (baseContract != null)
                        {
                            await _contractRepository.UpdateContractStatus(baseContract.ContractId, ContractStatusEnum.Completed.ToString());
                        }

                        await _contractRepository.UpdateContractStatus(contractId, ContractStatusEnum.Renting.ToString());
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(ex.Message);
                }
            }
        }

        public async Task CompleteContractOnTimeAsync(string contractId)
        {
            await CompleteContractOnTime(contractId);
        }
        public async Task CompleteContractOnTime(string contractId)
        {
            var contract = await _contractRepository.GetContractById(contractId);
            if (contract != null && contract.Status.Equals(ContractStatusEnum.Renting.ToString()))
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var currentDate = DateTime.Now.Date;
                    var actualRentPeriod = (currentDate - contract.DateStart.Value.Date).Days;
                    if (actualRentPeriod < 0)
                    {
                        actualRentPeriod = 0;
                    }

                    var updatedContract = await _contractRepository.EndContract(contractId, ContractStatusEnum.InspectionPending.ToString(), actualRentPeriod, currentDate);

                    await UpdateRentDaysCounterMachineSerialNumber(contract.SerialNumber, actualRentPeriod, (int)contract.AccountSignId);
                    //await _machineSerialNumberRepository.UpdateRentDaysCounterMachineSerialNumber(contract.SerialNumber, actualRentPeriod);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(ex.Message);
                }
            }
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
                    await _machineSerialNumberRepository.UpdateStatus(contract.SerialNumber, MachineSerialNumberStatusEnum.Available.ToString(), (int)contract.AccountSignId, null, null);
                }

                await _rentingRequestRepository.CancelRentingRequest(rentingRequestId);
            }
        }


        private async Task UpdateRentDaysCounterMachineSerialNumber(string serialNumber, int actualRentPeriod, int accountId)
        {
            var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(serialNumber);
            if (machineSerialNumber != null)
            {
                machineSerialNumber.RentDaysCounter = (machineSerialNumber.RentDaysCounter ?? 0) + actualRentPeriod;

                var machineSetting = await _settingsService.GetMachineSettingsAsync();
                var condition = machineSetting.DaysData
                    .OrderByDescending(d => d.RentedDays)
                    .FirstOrDefault(d => machineSerialNumber.RentDaysCounter >= d.RentedDays);

                if (condition != null)
                {
                    machineSerialNumber.MachineConditionPercent = condition.MachineConditionPercent;
                }

                if (machineSerialNumber.MachineConditionPercent.HasValue)
                {
                    var rate = machineSetting.RateData
                        .OrderByDescending(r => r.MachineConditionPercent)
                        .FirstOrDefault(r => machineSerialNumber.MachineConditionPercent >= r.MachineConditionPercent);

                    var machine = await _machineRepository.GetMachine((int)machineSerialNumber.MachineId);

                    if (machine != null && rate != null)
                    {
                        machineSerialNumber.ActualRentPrice = machine.RentPrice * (rate.RentalPricePercent / 100.0);
                    }
                }

                await _machineSerialNumberRepository.UpdateRentDaysCounterMachineSerialNumber(machineSerialNumber, accountId);
            }
        }
    }
}
