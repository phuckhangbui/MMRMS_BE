using DTOs.ComponentReplacementTicket;

namespace Repository.Interface
{
    public interface IComponentReplacementTicketRepository
    {
        Task<ComponentReplacementTicketDto> CreateTicket(int staffId, ComponentReplacementTicketDto componentReplacementTicketDto);
        Task<ComponentReplacementTicketDto> GetTicket(string ComponentReplacementTicketId);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetTickets();
        Task<IEnumerable<ComponentReplacementTicketDto>> GetTicketsByCustomerId(int customerId);
    }
}
