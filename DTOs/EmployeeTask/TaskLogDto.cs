namespace DTOs.EmployeeTask
{
    public class TaskLogDto
    {
        public int TaskLogId { get; set; }

        public int? EmployeeTaskId { get; set; }

        public int? AccountTriggerId { get; set; }

        public int? AccountTriggerName { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }
    }
}
