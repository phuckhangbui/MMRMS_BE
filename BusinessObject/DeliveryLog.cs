namespace BusinessObject
{
    public partial class DeliveryLog
    {
        public int DeliveryLogId { get; set; }

        public int? DeliveryId { get; set; }

        public int? AccountId { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }

        public virtual Delivery? Delivery { get; set; }

        public virtual Account? AccountTrigger { get; set; }
    }
}
