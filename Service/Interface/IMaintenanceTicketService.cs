using DTOs.ComponentReplacementTicket;

namespace Service.Interface
{
    public interface IComponentReplacementTicketService
    {
        Task CreateComponentReplacementTicket(int staffId, CreateComponentReplacementTicketDto createComponentReplacementTicketDto);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets();
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets(int customerId);
    }
}
