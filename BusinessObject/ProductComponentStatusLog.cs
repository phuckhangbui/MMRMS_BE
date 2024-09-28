namespace BusinessObject
{
    public class ProductComponentStatusLog
    {
        public int ProductComponentStatusLogId { get; set; }
        public int? ProductComponentStatusId { get; set; }
        public string? Type { get; set; }
        public DateTime? DateCreate { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public virtual ProductComponentStatus? ProductComponentStatus { get; set; }
    }
}
