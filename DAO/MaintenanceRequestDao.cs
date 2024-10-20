using BusinessObject;
using Microsoft.EntityFrameworkCore;

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
    }
}
