namespace DTOs.SerialNumberProduct
{
    public class SerialNumberProductLogDto
    {
        public int SerialNumberProductLogId { get; set; }
        public string? SerialNumber { get; set; }
        public int? AccountTriggerId { get; set; }
        public string? AccountTriggerName { get; set; }
        public string? Type { get; set; }
        public DateTime? DateCreate { get; set; }
        public string? Action { get; set; }
    }
}
