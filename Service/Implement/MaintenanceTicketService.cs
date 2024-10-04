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
            await _maintenanceTicketRepository.CreateTicket(staffId, createMaintenanceTicketDto);
        }

        public async Task<IEnumerable<MaintenanceTicketDto>> GetMaintenanceTickets()
        {
            return await _maintenanceTicketRepository.GetTickets();
        }

        public async Task<IEnumerable<MaintenanceTicketDto>> GetMaintenanceTickets(int customerId)
        {
            return await _maintenanceTicketRepository.GetTicketsByCustomerId(customerId);
        }
    }
}
