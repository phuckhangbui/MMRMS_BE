namespace BusinessObject
{
    public partial class DeliveryTaskLog
    {
        public int DeliveryTaskLogId { get; set; }

        public int? DeliveryTaskId { get; set; }

        public int? AccountTriggerId { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }

        public virtual DeliveryTask? DeliveryTask { get; set; }

        public virtual Account? AccountTrigger { get; set; }
    }
}
