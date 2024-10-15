namespace BusinessObject
{
    public class SerialNumberProductLog
    {
        public int SerialNumberProductLogId { get; set; }
        public string? SerialNumber { get; set; }
        public int? AccountTriggerId { get; set; }
        public string? Type { get; set; }
        public DateTime? DateCreate { get; set; }
        public string? Action { get; set; }
        public virtual SerialNumberProduct? SerialNumberProduct { get; set; }
        public virtual Account? AccountTrigger { get; set; }
    }
}
