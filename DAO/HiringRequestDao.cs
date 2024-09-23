﻿using BusinessObject;
using DAO.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class HiringRequestDao : BaseDao<HiringRequest>
    {
        private static HiringRequestDao instance = null;
        private static readonly object instacelock = new object();

        private HiringRequestDao()
        {

        }

        public static HiringRequestDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HiringRequestDao();
                }
                return instance;
            }
        }

        public async Task<HiringRequest> GetHiringRequestById(string hiringRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.HiringRequests
                    .FirstOrDefaultAsync(h => h.HiringRequestId.Equals(hiringRequestId));
            }
        }

        public async Task<HiringRequest> GetHiringRequestByIdAndStatus(string hiringRequestId, string status)
        {
            using (var context = new MmrmsContext())
            {
                return await context.HiringRequests
                    .FirstOrDefaultAsync(h => h.HiringRequestId.Equals(hiringRequestId) 
                        && h.Status.Equals(status));
            }
        }
    }
}
