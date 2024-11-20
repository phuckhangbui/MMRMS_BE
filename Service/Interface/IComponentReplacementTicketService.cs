using DTOs.ComponentReplacementTicket;

namespace Service.Interface
{
    public interface IComponentReplacementTicketService
    {
        Task CancelComponentReplacementTicket(int customerId, string componentReplacementTicketId);
        Task CompleteComponentReplacementTicket(int staffId, string componentReplacementTicketId);
        Task CreateComponentReplacementTicket(int staffId, CreateComponentReplacementTicketDto createComponentReplacementTicketDto);
        Task<ComponentReplacementTicketDetailDto> GetComponentReplacementTicket(string replacementTicketId);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets();
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTickets(int customerId);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTicketsByContractId(string contractId);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTicketsBySerialNumber(string serialNumber);
        Task<IEnumerable<ComponentReplacementTicketDto>> GetComponentReplacementTicketsForStaff(int staffId);
    }
}
