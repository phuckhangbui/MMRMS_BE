﻿using BusinessObject;
using Microsoft.EntityFrameworkCore;
using MachineCheckRequest = BusinessObject.MachineCheckRequest;

namespace DAO
{
    public class MachineCheckRequestDao : BaseDao<MachineCheckRequest>
    {
        private static MachineCheckRequestDao instance = null;
        private static readonly object instacelock = new object();

        private MachineCheckRequestDao()
        {

        }

        public static MachineCheckRequestDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineCheckRequestDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<MachineCheckRequest>> GetMachineCheckRequests()
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineCheckRequests
                    .Include(c => c.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .Include(c => c.Contract)
                    .ThenInclude(c => c.ContractMachineSerialNumber)
                    .ThenInclude(s => s.Machine)
                    .ThenInclude(m => m.MachineImages)
                    .OrderByDescending(c => c.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<MachineCheckRequest> GetMachineCheckRequest(string MachineCheckRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineCheckRequests
                    .Include(c => c.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .FirstOrDefaultAsync(m => m.MachineCheckRequestId == MachineCheckRequestId);
            }
        }

        public async Task<MachineCheckRequest> GetMachineCheckRequestDetail(string machineCheckRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineCheckRequests
                    .Include(c => c.MachineCheckRequestCriterias)
                    .Include(c => c.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .Include(c => c.MachineTask)
                    .ThenInclude(t => t.ComponentReplacementTicketsCreateFromTask)
                        .ThenInclude(t => t.Component)
                    .Include(c => c.MachineTask)
                    .ThenInclude(t => t.ComponentReplacementTicketsCreateFromTask)
                        .ThenInclude(t => t.EmployeeCreate)
                    .Include(c => c.Contract)
                    .ThenInclude(c => c.ContractMachineSerialNumber)
                    .ThenInclude(s => s.Machine)
                    .ThenInclude(m => m.MachineImages)
                    .FirstOrDefaultAsync(m => m.MachineCheckRequestId == machineCheckRequestId);
            }
        }

        public async Task<MachineCheckRequest> CreateMachineCheckRequest(MachineCheckRequest request, IEnumerable<MachineCheckRequestCriteria> machineCheckRequestCriterias)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        context.MachineCheckRequests.Add(request);
                        await context.SaveChangesAsync();

                        foreach (var criteria in machineCheckRequestCriterias)
                        {
                            context.MachineCheckRequestCriterias.Add(criteria);
                        }
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return request;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Error occurred during transaction: " + ex.Message);
                    }
                }
            }
        }

        public async Task<int> GetTotalRequestByDate(DateTime date)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineCheckRequests
                    .Where(r => r.DateCreate.HasValue && r.DateCreate.Value.Date == date.Date)
                    .CountAsync();
            }
        }

        public async Task<int> GetMachineCheckRequestsInRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            using var context = new MmrmsContext();
            IQueryable<MachineCheckRequest> query = context.MachineCheckRequests;

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
