﻿using BusinessObject;
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

        public async Task<Contract> GetContractById(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                     .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusinesses)
                    .Include(c => c.ContractTerms)
                    .FirstOrDefaultAsync(c => c.ContractId == contractId);
            }
        }

        public async Task<IEnumerable<Contract>> GetContractsForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Where(c => c.AccountSignId == customerId)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetContracts()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusinesses)
                    .Include(c => c.ContractTerms)
                    .ToListAsync();
            }
        }
    }
}
