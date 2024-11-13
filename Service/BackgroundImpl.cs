using Common.Enum;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
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

        public BackgroundImpl(ILogger<BackgroundImpl> logger,
            IRentingRequestRepository rentingRequestRepository,
            IContractRepository contractRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IInvoiceRepository invoiceRepository)
        {
            _logger = logger;
            _rentingRequestRepository = rentingRequestRepository;
            _contractRepository = contractRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _invoiceRepository = invoiceRepository;
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

        public void ProcessExtendContractJob(string contractId, TimeSpan delayToStart)
        {
            try
            {
                _logger.LogInformation($"Starting ProcessExtendContractJob");

                BackgroundJob.Schedule(() => ProcessExtendContracAsync(contractId), delayToStart);
                _logger.LogInformation($"Contract: {contractId} scheduled for status change: {ContractStatusEnum.Completed.ToString()} at {delayToStart}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while ProcessExtendContractJob");
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
                        await _contractRepository.UpdateContractStatus(contractId, ContractStatusEnum.Canceled.ToString());
                        foreach (var contractPayment in extendContract.ContractPayments)
                        {
                            contractPayment.Status = ContractPaymentStatusEnum.Canceled.ToString();
                            await _contractRepository.UpdateContractPayment(contractPayment);

                            var invoice = await _invoiceRepository.GetInvoice(contractPayment.InvoiceId);
                            if (invoice != null)
                            {
                                await _invoiceRepository.UpdateInvoiceStatus(invoice.InvoiceId, InvoiceStatusEnum.Canceled.ToString());
                            }
                        }
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

                    await _machineSerialNumberRepository.UpdateRentDaysCounterMachineSerialNumber(contract.SerialNumber, actualRentPeriod);

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
