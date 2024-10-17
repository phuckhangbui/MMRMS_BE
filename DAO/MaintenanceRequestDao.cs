﻿using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class MaintenanceRequestDao : BaseDao<MaintenanceRequest>
    {
        private static MaintenanceRequestDao instance = null;
        private static readonly object instacelock = new object();

        private MaintenanceRequestDao()
        {

        }

        public static MaintenanceRequestDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaintenanceRequestDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetMaintenanceRequests()
        {
            using (var context = new MmrmsContext())
            {
                return await context.MaintenanceRequests
                    .Include(c => c.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .ToListAsync();
            }
        }

        public async Task<MaintenanceRequest> GetMaintenanceRequest(string maintenanceRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.MaintenanceRequests
                    .Include(c => c.Contract)
                    .ThenInclude(c => c.RentingRequest)
                    .ThenInclude(c => c.RentingRequestAddress)
                    .FirstOrDefaultAsync(m => m.RequestId == maintenanceRequestId);
            }
        }
    }
}
