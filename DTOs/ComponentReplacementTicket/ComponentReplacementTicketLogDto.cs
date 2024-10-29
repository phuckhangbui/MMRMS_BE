namespace DTOs.ComponentReplacementTicket
{
    public class ComponentReplacementTicketLogDto
    {
        public int ComponentReplacementTicketLogId { get; set; }

        public string? ComponentReplacementTicketId { get; set; }

        public int? AccountTriggerId { get; set; }

        public string? AccountTriggerName { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }
    }
}
