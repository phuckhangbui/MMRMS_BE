using DTOs.ComponentReplacementTicket;

namespace Service.Interface
{
    public interface IComponentReplacementTicketService
    {
        Task CompleteComponentReplacementTicket(int staffId, string componentReplacementTicketId);
        Task CreateComponentReplacementTicketWhenCheckMachineRenting(int staffId, CreateComponentReplacementTicketDto createComponentReplacementTicketDto);
        Task<ComponentReplacementTicketDetailDto> GetComponentReplacementTicket(string replacementTicketId);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets();
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets(int customerId);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTicketsForStaff(int staffId);
    }
}
