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

                if (IsRentingRequestValidToCancel(rentingRequest))
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

        private bool IsRentingRequestValidToCancel(RentingRequest rentingRequest)
        {
            if (rentingRequest == null || !rentingRequest.Status.Equals(RentingRequestStatusEnum.UnPaid.ToString()))
            {
                return false;
            }

            foreach (var contract in rentingRequest.Contracts)
            {
                var isPaid = contract.ContractPayments
                    .Any(c => c.Status.Equals(ContractPaymentStatusEnum.Paid.ToString()));

                if (isPaid)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
