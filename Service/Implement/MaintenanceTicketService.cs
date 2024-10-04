using DTOs.MaintenanceTicket;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class MaintenanceTicketService : IMaintenanceTicketService
    {
        private readonly IMaintenanceTicketRepository _maintenanceTicketRepository;

        public async Task CreateMaintenanceRequest(int staffId, CreateMaintenanceTicketDto createMaintenanceTicketDto)
        {

        }

        public Task<IEnumerable<MaintenanceTicketDto>> GetMaintenanceTickets()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintenanceTicketDto>> GetMaintenanceTickets(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
