using BusinessObject;
using Microsoft.EntityFrameworkCore;
using ComponentReplacementTicket = BusinessObject.ComponentReplacementTicket;

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
                                    .Include(c => c.EmployeeCreate)
                                    .Include(c => c.Component)
                                    .Include(c => c.Contract)
                                    .OrderByDescending(c => c.DateCreate)
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<ComponentReplacementTicket>> GetComponentReplacementTicketByContractId(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.ComponentReplacementTickets
                                    .Where(c => c.ContractId == contractId)
                                    .OrderByDescending(c => c.DateCreate)
                                    .ToListAsync();
            }
        }

        public async Task<ComponentReplacementTicket> GetComponentReplacementTicket(string ticketId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.ComponentReplacementTickets
                                    .Include(c => c.EmployeeCreate)
                                    .Include(c => c.Component)
                                    .Include(c => c.Contract)
                                    .FirstOrDefaultAsync(c => c.ComponentReplacementTicketId == ticketId);
            }
        }

        public async Task<ComponentReplacementTicket> GetComponentReplacementTicketDetail(string ticketId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.ComponentReplacementTickets
                                    .Include(c => c.EmployeeCreate)
                                    .Include(c => c.Component)
                                    .Include(c => c.Contract)
                                    .Include(d => d.ComponentReplacementTicketLogs.OrderByDescending(l => l.DateCreate))
                                    .ThenInclude(l => l.AccountTrigger)
                                    .FirstOrDefaultAsync(c => c.ComponentReplacementTicketId == ticketId);
            }
        }


        //have transaction inside the service layer
        public async Task<ComponentReplacementTicket> CreateTicket(ComponentReplacementTicket componentTicket, ComponentReplacementTicketLog ticketLog)
        {
            using (var context = new MmrmsContext())
            {

                context.Entry(componentTicket).State = EntityState.Added;
                context.ComponentReplacementTickets.Add(componentTicket);
                await context.SaveChangesAsync();

                context.ComponentReplacementTicketLogs.Add(ticketLog);
                await context.SaveChangesAsync();

                return componentTicket;
            }
        }

        public async Task<int> GetLatestTicketByDate(DateTime date)
        {
            using (var context = new MmrmsContext())
            {
                var ids = await context.ComponentReplacementTickets
                    .Where(t => t.DateCreate.HasValue && t.DateCreate.Value.Date == date.Date)
                    .Where(t => t.ComponentReplacementTicketId != null)
                    .Select(t => t.ComponentReplacementTicketId)
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
