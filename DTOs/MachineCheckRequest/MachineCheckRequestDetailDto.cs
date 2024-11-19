using DTOs.ComponentReplacementTicket;

namespace DTOs.MachineCheckRequest
{
    public class MachineCheckRequestDetailDto
    {
        public MachineCheckRequestDto MachineCheckRequest { get; set; }

        public IEnumerable<MachineCheckRequestCriteriaDto> CheckCriteriaList { get; set; } = new List<MachineCheckRequestCriteriaDto>();

        public IEnumerable<ComponentReplacementTicketDto> ComponentReplacementTickets { get; set; } = new List<ComponentReplacementTicketDto>();
    }
}
