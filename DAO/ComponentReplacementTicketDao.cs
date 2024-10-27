using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ComponentReplacementTicketDao : BaseDao<ComponentReplacementTicket>
    {
        private static ComponentReplacementTicketDao instance = null;
        private static readonly object instacelock = new object();

        private ComponentReplacementTicketDao()
        {

        }

        public static ComponentReplacementTicketDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentReplacementTicketDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<ComponentReplacementTicket>> GetComponentReplacementTickets()
        {
            using (var context = new MmrmsContext())
            {
                return await context.ComponentReplacementTickets
                    .Include(c => c.EmployeeCreate).Include(c => c.Component).Include(c => c.Contract)
                    .ToListAsync();
            }
        }
    }
}
