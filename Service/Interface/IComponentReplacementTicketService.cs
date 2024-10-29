using DTOs.ComponentReplacementTicket;

namespace Service.Interface
{
    public interface IComponentReplacementTicketService
    {
        Task CreateComponentReplacementTicketWhenCheckMachineRenting(int staffId, CreateComponentReplacementTicketDto createComponentReplacementTicketDto);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets();
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets(int customerId);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTicketsForStaff(int staffId);
    }
}
