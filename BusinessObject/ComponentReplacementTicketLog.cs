namespace BusinessObject
{
    public class ComponentReplacementTicketLog
    {
        public int ComponentReplacementTicketLogId { get; set; }

        public string? ComponentReplacementTicketId { get; set; }

        public int? AccountTriggerId { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }

        public virtual ComponentReplacementTicket? ComponentReplacementTicket { get; set; }

        public virtual Account? AccountTrigger { get; set; }
    }
}
