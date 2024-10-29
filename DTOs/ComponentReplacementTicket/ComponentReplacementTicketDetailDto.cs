namespace DTOs.ComponentReplacementTicket
{
    public class ComponentReplacementTicketDetailDto
    {
        public ComponentReplacementTicketDto ComponentReplacementTicket { get; set; }

        public IEnumerable<ComponentReplacementTicketLogDto> Logs { get; set; }
    }
}
