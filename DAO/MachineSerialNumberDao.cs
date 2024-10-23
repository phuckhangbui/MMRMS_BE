using Common.Enum;
using Microsoft.EntityFrameworkCore;
using MachineSerialNumber = BusinessObject.MachineSerialNumber;

namespace DAO
{
    public class MachineSerialNumberDao : BaseDao<MachineSerialNumber>
    {
        private static MachineSerialNumberDao instance = null;
        private static readonly object instacelock = new object();

        private MachineSerialNumberDao()
        {

        }

        public static MachineSerialNumberDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineSerialNumberDao();
                }
                return instance;
            }
        }

        public async Task<MachineSerialNumber> GetMachineSerialNumberBySerialNumberAndMachineId(string serialNumber, int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineSerialNumbers
                    .FirstOrDefaultAsync(s => s.MachineId == productId && s.SerialNumber.Equals(serialNumber));
            }
        }

        public async Task<bool> IsMachineSerialNumberValidToRent(string serialNumber, int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineSerialNumbers
                    .AnyAsync(s => s.MachineId == productId
                            && s.SerialNumber.Equals(serialNumber)
                            && s.Status == MachineSerialNumberStatusEnum.Available.ToString());
            }
        }

        public async Task<bool> IsSerialNumberExisted(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineSerialNumbers
                    .AnyAsync(s => s.SerialNumber.Equals(serialNumber));
            }
        }

        public async Task<IEnumerable<MachineSerialNumber>> GetMachineSerialNumbersByMachineIdAndStatus(int productId, string status)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineSerialNumbers
                    .Where(s => s.MachineId == productId && s.Status.Equals(status))
                    .OrderByDescending(s => s.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<bool> IsSerialNumberInAnyContract(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .AnyAsync(s => s.SerialNumber.Equals(serialNumber));
            }
        }

        public async Task Delete(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                var machineSerialNumber = await context.MachineSerialNumbers
                    .FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                if (machineSerialNumber != null)
                {
                    DbSet<MachineSerialNumber> _dbSet = context.Set<MachineSerialNumber>();
                    _dbSet.Remove(machineSerialNumber);
                    await context.SaveChangesAsync();
                }
            }
        }

        


        public async Task<MachineSerialNumber> GetMachineSerialNumber(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                var machineSerialNumber = await context.MachineSerialNumbers
                    .FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                return machineSerialNumber;
            }
        }

        public async Task<MachineSerialNumber> GetMachineSerialNumberDetail(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                var machineSerialNumber = await context.MachineSerialNumbers.Include(c => c.MachineSerialNumberLogs).ThenInclude(l => l.AccountTrigger).Include(c => c.MachineComponentStatuses).ThenInclude(c => c.Component).ThenInclude(c => c.Component).FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                return machineSerialNumber;
            }
        }

        //public async Task CreateSerialMachine(MachineSerialNumber serialMachine)
        //{
        //    using (var context = new MmrmsContext())
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                DbSet<MachineSerialNumber> _dbSet = context.Set<MachineSerialNumber>();
        //                _dbSet.Add(serialMachine);
        //                await context.SaveChangesAsync();

        //                var machineSerialNumber

        //                await transaction.CommitAsync();
        //            }
        //            catch (Exception e)
        //            {
        //                transaction.Rollback();
        //                throw new Exception(e.Message);
        //            }
        //        }
        //    }
        //}

        public async Task<bool> IsMachineSerialNumberValidToRent(int productId, int quantity, DateTime startDate, int numberOfMonth)
        {
            using var context = new MmrmsContext();

            var requestedEndDate = startDate.AddMonths(numberOfMonth);

            var availableSerialNumbers = await context.MachineSerialNumbers
                .Where(s => s.MachineId == productId
                        && s.Status == MachineSerialNumberStatusEnum.Available.ToString())
                .Select(s => s.SerialNumber)
                .ToListAsync();

            var serialNumbersInFutureContracts = await context.Contracts
                .Where(c => availableSerialNumbers.Contains(c.SerialNumber!)
                        && (c.Status == ContractStatusEnum.NotSigned.ToString() || c.Status == ContractStatusEnum.Signed.ToString() || c.Status == ContractStatusEnum.Shipping.ToString() || c.Status == ContractStatusEnum.Shipped.ToString())
                        && (c.DateStart < requestedEndDate && c.DateEnd > startDate))
                .Select(c => c.SerialNumber)
                .ToListAsync();

            var suitableAvailableSerialNumbers = availableSerialNumbers
                .Except(serialNumbersInFutureContracts)
                .ToList();

            return suitableAvailableSerialNumbers.Count >= quantity;
        }

        public async Task<List<MachineSerialNumber>> GetMachineSerialNumberValidToRent(int productId, DateTime startDate, int numberOfMonth)
        {
            using var context = new MmrmsContext();

            var requestedEndDate = startDate.AddMonths(numberOfMonth);

            var availableSerialNumbers = await context.MachineSerialNumbers
                .Where(s => s.MachineId == productId
                        && s.Status == MachineSerialNumberStatusEnum.Available.ToString())
                .ToListAsync();

            var serialNumbersInFutureContracts = await context.Contracts
                .Where(c => availableSerialNumbers.Contains(c.ContractMachineSerialNumber)
                        && (c.Status == ContractStatusEnum.Signed.ToString() || c.Status == ContractStatusEnum.Shipping.ToString() || c.Status == ContractStatusEnum.Shipped.ToString())
                        && (c.DateStart < requestedEndDate && c.DateEnd > startDate))
                .Select(c => c.ContractMachineSerialNumber)
                .ToListAsync();

            var suitableAvailableSerialNumbers = availableSerialNumbers
                .Except(serialNumbersInFutureContracts)
                .ToList();

            return suitableAvailableSerialNumbers;
        }
    }
}
