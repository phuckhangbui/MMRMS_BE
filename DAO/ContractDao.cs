using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ContractDao : BaseDao<Contract>
    {
        private static ContractDao instance = null;
        private static readonly object instacelock = new object();

        private ContractDao()
        {

        }

        public static ContractDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContractDao();
                }
                return instance;
            }
        }

        public async Task<Contract> GetContractDetailById(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                     .Include(c => c.RentingRequest)
                        .ThenInclude(r => r.RentingRequestAddress)
                     .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusiness)
                    .Include(c => c.ContractTerms)
                    .Include(c => c.ContractPayments)
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(a => a.Machine)
                        .ThenInclude(m => m.MachineImages)
                    .FirstOrDefaultAsync(c => c.ContractId == contractId);
            }
        }

        public async Task<Contract?> GetContractById(string contractId)
        {
            using var context = new MmrmsContext();
            return await context.Contracts
                .Include(c => c.ContractPayments)
                .Include(c => c.ContractMachineSerialNumber)
                .FirstOrDefaultAsync(c => c.ContractId == contractId);
        }

        public async Task<IEnumerable<Contract>> GetContractsByRentingRequestId(string rentingRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Where(c => c.RentingRequestId.Equals(rentingRequestId))
                     .Include(c => c.RentingRequest)
                        .ThenInclude(r => r.RentingRequestAddress)
                     .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusiness)
                    .Include(c => c.ContractTerms)
                    .Include(c => c.ContractPayments)
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(a => a.Machine)
                        .ThenInclude(m => m.MachineImages)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetContractsForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Where(c => c.AccountSignId == customerId)
                    .Include(c => c.RentingRequest)
                        .ThenInclude(r => r.RentingRequestAddress)
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(a => a.Machine)
                        .ThenInclude(m => m.MachineImages)
                    .OrderByDescending(c => c.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetContracts()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(s => s.Machine)
                    .ThenInclude(m => m.MachineImages)
                    .OrderByDescending(c => c.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetRentalHistoryOfSerialNumber(string serialNumber)
        {
            using var context = new MmrmsContext();
            return await context.Contracts
                .Where(c => c.SerialNumber.Equals(serialNumber))
                .Include(c => c.ContractMachineSerialNumber)
                    .ThenInclude(s => s.Machine)
                .ThenInclude(m => m.MachineImages)
                .OrderByDescending(c => c.DateCreate)
                .ToListAsync();
        }

        public async Task<RentingRequestAddress?> GetRentingRequestAddressByContractId(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                var contract = await context.Contracts.FirstOrDefaultAsync(c => c.ContractId == contractId);

                if (contract == null)
                {
                    return null;
                }

                var rentingRequest = await context.RentingRequests.Include(r => r.RentingRequestAddress).FirstOrDefaultAsync(r => r.RentingRequestId == contract.RentingRequestId);

                if (rentingRequest == null)
                {
                    return null;
                }

                return rentingRequest.RentingRequestAddress;
            }
        }

        public async Task<int> GetTotalContractByDate(DateTime date)
        {
            using var context = new MmrmsContext();
            return await context.Contracts
                .Where(r => r.DateCreate.HasValue && r.DateCreate.Value.Date == date.Date)
                .CountAsync();
        }

        public async Task<int> GetContractsInRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            using var context = new MmrmsContext();
            IQueryable<Contract> query = context.Contracts;

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

        public async Task<Contract?> GetExtendContract(string baseContractId)
        {
            using var context = new MmrmsContext();
            return await context.Contracts
                .FirstOrDefaultAsync(c => c.BaseContractId.Equals(baseContractId));
        }

        public async Task<int> GetLastestContractByDate(DateTime date)
        {
            using (var context = new MmrmsContext())
            {
                var ids = await context.Contracts
                    .Where(t => t.DateCreate.HasValue && t.DateCreate.Value.Date == date.Date)
                    .Where(t => t.ContractId != null)
                    .Select(t => t.ContractId)
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
    }
}
