using DTOs.MaintainingTicket;

namespace Service.Interface
{
    public interface IMaintenanceTicketService
    {
        Task CreateMaintenanceRequest(int staffId, CreateMaintaningTicketDto createMaintaningTicketDto);
        Task<IEnumerable<MaintaningTicketDto>> GetMaintenanceTickets();
        Task<IEnumerable<MaintaningTicketDto>> GetMaintenanceTickets(int customerId);
    }
}
