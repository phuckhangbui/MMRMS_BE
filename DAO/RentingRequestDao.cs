﻿using BusinessObject;
using Common.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class RentingRequestDao : BaseDao<RentingRequest>
    {
        private static RentingRequestDao instance = null;
        private static readonly object instacelock = new object();

        private RentingRequestDao()
        {

        }

        public static RentingRequestDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RentingRequestDao();
                }
                return instance;
            }
        }

        public async Task<RentingRequest> GetRentingRequestById(string rentingRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .Include(rr => rr.ServiceRentingRequests)
                        .ThenInclude(rr => rr.RentingService)
                    .Include(rr => rr.AccountOrder)
                    .Include(rr => rr.Contracts)
                        .ThenInclude(c => c.ContractMachineSerialNumber)
                        .ThenInclude(s => s.Machine)
                        .ThenInclude(p => p.MachineImages)
                    .Include(rr => rr.RentingRequestAddress)
                    .FirstOrDefaultAsync(rr => rr.RentingRequestId.Equals(rentingRequestId));
            }
        }

        public async Task<RentingRequest> GetRentingRequestByContractId(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .Include(rr => rr.Contracts)
                    .FirstOrDefaultAsync(rr => rr.Contracts.Any(c => c.ContractId == contractId));
            }
        }


        public async Task<RentingRequest> GetRentingRequest(string rentingRequestId)
        {
            using var context = new MmrmsContext();
            return await context.RentingRequests
                .Include(rr => rr.Contracts)
                    .ThenInclude(c => c.ContractPayments)
                .FirstOrDefaultAsync(rr => rr.RentingRequestId.Equals(rentingRequestId));
        }

        public async Task<IEnumerable<RentingRequest>> GetRentingRequests()
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .Include(h => h.AccountOrder)
                    .OrderByDescending(rr => rr.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<RentingRequest>> GetRentingRequestsForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .Where(rr => rr.AccountOrderId == customerId)
                    .Include(h => h.AccountOrder)
                    .OrderByDescending(rr => rr.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<RentingRequest?> CancelRentingRequest(string rentingRequestId)
        {
            using var context = new MmrmsContext();
            try
            {
                var rentingRequest = await context.RentingRequests
                    .Include(rr => rr.Contracts)
                        .ThenInclude(c => c.ContractPayments)
                        .ThenInclude(cp => cp.Invoice)
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

                            if (contractPayment.Invoice != null)
                            {
                                contractPayment.Invoice.Status = InvoiceStatusEnum.Canceled.ToString();
                            }
                        }

                        context.Contracts.Update(contract);
                    }

                    context.RentingRequests.Update(rentingRequest);

                    await context.SaveChangesAsync();
                }

                return rentingRequest;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<int> GetLastestRentingRequestByDate(DateTime date)
        {
            using (var context = new MmrmsContext())
            {
                var ids = await context.RentingRequests
                    .Where(t => t.DateCreate.HasValue && t.DateCreate.Value.Date == date.Date)
                    .Where(t => t.RentingRequestId != null)
                    .Select(t => t.RentingRequestId)
                    .ToListAsync();

                if (!ids.Any())
                {
                    return 0;
                }

                var latestId = ids
                     .Select(id =>
                     {
                         int startIndex = id.IndexOf("NO") + 2;
                         return int.Parse(id.Substring(startIndex));
                     })
                     .OrderByDescending(id => id)
                     .FirstOrDefault();

                return latestId;
            }
        }

        public async Task<int> GetRentingRequestsInRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            using var context = new MmrmsContext();
            IQueryable<RentingRequest> query = context.RentingRequests;

            if (startDate.HasValue)
            {
                query = query.Where(a => a.DateCreate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.DateCreate <= endDate.Value);
            }

            return await query.CountAsync();
        }
    }
}
