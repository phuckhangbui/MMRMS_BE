namespace DTOs.MachineTask
{
    public class MachineTaskLogDto
    {
        public int MachineTaskLogId { get; set; }

        public int? MachineTaskId { get; set; }

        public int? AccountTriggerId { get; set; }

        public string? AccountTriggerName { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }
    }
}
