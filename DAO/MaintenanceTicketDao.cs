using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class MaintenanceTicketDao : BaseDao<MaintenanceTicket>
    {
        private static MaintenanceTicketDao instance = null;
        private static readonly object instacelock = new object();

        private MaintenanceTicketDao()
        {

        }

        public static MaintenanceTicketDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaintenanceTicketDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<MaintenanceTicket>> GetMaintenanceTickets()
        {
            using (var context = new MmrmsContext())
            {
                return await context.MaintenanceTickets
                    .Include(c => c.EmployeeCreate).Include(c => c.Component).Include(c => c.Contract)
                    .ToListAsync();
            }
        }
    }
}
