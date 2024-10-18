namespace BusinessObject
{
    public class MaintenanceTicketLog
    {
        public int MaintenanceTicketLogId { get; set; }

        public string? MaintenanceTicketId { get; set; }

        public int? AccountTriggerId { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }

        public virtual MaintenanceTicket? MaintenanceTicket { get; set; }

        public virtual Account? AccountTrigger { get; set; }
    }
}
