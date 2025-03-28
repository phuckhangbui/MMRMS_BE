﻿using BusinessObject;
using Common.Enum;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> IsSerialNumberExisted(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineSerialNumbers
                    .AnyAsync(s => s.SerialNumber.Equals(serialNumber));
            }
        }

        public async Task<IEnumerable<MachineSerialNumber>> GetMachineSerialNumbersByMachineId(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MachineSerialNumbers
                    .Where(s => s.MachineId == productId)
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
                    .Include(m => m.Machine)
                        .ThenInclude(m => m.MachineTerms)
                    .FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                return machineSerialNumber;
            }
        }

        public async Task<MachineSerialNumber> GetMachineSerialNumberDetail(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                var machineSerialNumber = await context.MachineSerialNumbers
                                                       .Include(d => d.MachineSerialNumberLogs.OrderByDescending(l => l.DateCreate))
                                                       .ThenInclude(l => l.AccountTrigger)
                                                       .Include(c => c.MachineSerialNumberComponents)
                                                       .ThenInclude(c => c.MachineComponent)
                                                       .ThenInclude(c => c.Component)
                                                       .FirstOrDefaultAsync(s => s.SerialNumber.Equals(serialNumber));

                return machineSerialNumber;
            }
        }

        public async Task<List<MachineSerialNumber>> GetMachineSerialNumberAvailablesToRent(int machineId, DateTime startDate)
        {
            using var context = new MmrmsContext();
            var availableSerialNumbersToRent = await context.MachineSerialNumbers
                .Where(s => s.MachineId == machineId &&
                        (s.Status == MachineSerialNumberStatusEnum.Available.ToString() ||
                        (s.Status == MachineSerialNumberStatusEnum.Maintained.ToString()) && s.ExpectedAvailableDate != null && startDate >= s.ExpectedAvailableDate))
                .Include(s => s.Machine)
                    .ThenInclude(p => p.MachineTerms)
                .OrderByDescending(s => s.DateCreate)
                .ThenByDescending(s => s.RentDaysCounter)
                .ToListAsync();

            return availableSerialNumbersToRent;
        }
    }
}
