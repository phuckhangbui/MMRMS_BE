namespace DTOs.Log
{
    public class LogDto
    {
        public int LogId { get; set; }
        public int AccountLogId { get; set; }
        public DateTime DateUpdate { get; set; }
        public DateTime DateCreate { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class LogDetailDto
    {
        public int LogDetailId { get; set; }
        public int LogId { get; set; }
        public string Action { get; set; }
        public DateTime? DateCreate { get; set; }
    }
}
