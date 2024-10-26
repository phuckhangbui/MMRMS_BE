namespace DTOs.Delivery
{
    public class DeliveryTaskLogDto
    {
        public int DeliveryTaskLogId { get; set; }

        public int? DeliveryTaskId { get; set; }

        public int? AccountTriggerId { get; set; }

        public string? AccountTriggerName { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }
    }
}
