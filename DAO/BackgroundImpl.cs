using BusinessObject;
using Common;
using Common.Enum;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAO
{
    public class BackgroundImpl : IBackground
    {
        private readonly ILogger<BackgroundImpl> _logger;

        public BackgroundImpl(ILogger<BackgroundImpl> logger)
        {
            _logger = logger;
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

        public void GenerateInvoiceJob(int nextContractPaymentId, TimeSpan delayToStart)
        {
            try
            {
                _logger.LogInformation($"Starting ScheduleGenerateInvoiceJob");

                BackgroundJob.Schedule(() => GenerateInvoiceForRecurringPaymentAsync(nextContractPaymentId), delayToStart);
                _logger.LogInformation($"Invoice generation for contract payment {nextContractPaymentId} scheduled to run in {delayToStart.TotalMinutes} minutes.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while ScheduleGenerateInvoiceJob");
            }
        }

        public async Task CompleteContractOnTimeAsync(string contractId)
        {
            await CompleteContractOnTime(contractId);
        }
        public async Task CompleteContractOnTime(string contractId)
        {
            using var context = new MmrmsContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var contract = await context.Contracts.FirstOrDefaultAsync(c => c.ContractId.Equals(contractId));
                if (contract != null && contract.Status.Equals(ContractStatusEnum.Renting.ToString()))
                {
                    contract.Status = ContractStatusEnum.Completed.ToString();

                    //TODO: Send noti to manager to create delivery task for returning machine back

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }

        public async Task GenerateInvoiceForRecurringPaymentAsync(int nextContractPaymentId)
        {
            await GenerateInvoiceForRecurringPayment(nextContractPaymentId);
        }
        public async Task GenerateInvoiceForRecurringPayment(int nextContractPaymentId)
        {
            using var context = new MmrmsContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var nextContractPayment = await context.ContractPayments
                    .Include(cp => cp.Contract)
                    .FirstOrDefaultAsync(cp => cp.ContractPaymentId == nextContractPaymentId);

                if (nextContractPayment != null)
                {
                    var nextInvoice = new Invoice
                    {
                        InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "RENTAL" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                        Amount = nextContractPayment.Amount,
                        Type = InvoiceTypeEnum.Rental.ToString(),
                        Status = InvoiceStatusEnum.Pending.ToString(),
                        DateCreate = DateTime.Now,
                        AccountPaidId = nextContractPayment.Contract.AccountSignId,
                    };

                    nextContractPayment.Invoice = nextInvoice;

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }

        public async Task CancelRentingRequestAsync(string rentingRequestId)
        {
            await CancelRentingRequest(rentingRequestId);
        }
        public async Task CancelRentingRequest(string rentingRequestId)
        {
            using var context = new MmrmsContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var rentingRequest = await context.RentingRequests
                    .Include(rr => rr.Contracts)
                        .ThenInclude(c => c.ContractPayments)
                    .FirstOrDefaultAsync(rq => rq.RentingRequestId.Equals(rentingRequestId));

                if (rentingRequest != null)
                {
                    rentingRequest.Status = RentingRequestStatusEnum.Canceled.ToString();

                    foreach (var contract in rentingRequest.Contracts)
                    {
                        contract.Status = ContractStatusEnum.Canceled.ToString();

                        foreach (var contractPayment in contract.ContractPayments)
                        {
                            contractPayment.Status = ContractPaymentStatusEnum.Canceled.ToString();
                        }

                        context.Contracts.Update(contract);
                    }

                    context.RentingRequests.Update(rentingRequest);

                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }
    }
}
