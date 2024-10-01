using DTOs.MaintainingTicket;
using Service.Interface;

namespace Service.Implement
{
    public class MaintenanceTicketService : IMaintenanceTicketService
    {
        public Task CreateMaintenanceRequest(int staffId, CreateMaintaningTicketDto createMaintaningTicketDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintaningTicketDto>> GetMaintenanceTickets()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintaningTicketDto>> GetMaintenanceTickets(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
