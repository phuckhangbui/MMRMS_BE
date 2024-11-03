namespace DTOs.LogDto
{
    public class LogDetailDto
    {
        public int LogDetailId { get; set; }
        public int AccountId { get; set; }
        public string Action { get; set; }
        public DateTime? DateCreate { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
