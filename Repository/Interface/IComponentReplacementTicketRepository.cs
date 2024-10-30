using DTOs.ComponentReplacementTicket;

namespace Repository.Interface
{
    public interface IComponentReplacementTicketRepository
    {
        Task<ComponentReplacementTicketDto> CreateTicket(int staffId, ComponentReplacementTicketDto componentReplacementTicketDto, int? accountSignId);
        Task<ComponentReplacementTicketDetailDto> GetComponentReplacementTicketDetail(string replacementTicketId);
        Task<ComponentReplacementTicketDto> GetTicket(string ComponentReplacementTicketId);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetTickets();
        Task<IEnumerable<ComponentReplacementTicketDto>> GetTicketsByCustomerId(int customerId);
        Task UpdateTicketStatus(string componentTicketId, string status, int accountId);
        Task UpdateTicketStatusToComplete(string componentReplacementTicketId, int staffId);
    }
}
