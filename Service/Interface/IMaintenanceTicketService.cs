using DTOs.MaintenanceTicket;

namespace Service.Interface
{
    public interface IMaintenanceTicketService
    {
        Task CreateMaintenanceTicket(int staffId, CreateMaintenanceTicketDto createMaintenanceTicketDto);
        Task<IEnumerable<MaintenanceTicketDto>> GetMaintenanceTickets();
        Task<IEnumerable<MaintenanceTicketDto>> GetMaintenanceTickets(int customerId);
    }
}
