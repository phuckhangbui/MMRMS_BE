namespace DTOs.MachineComponentStatus
{
    public class MachineSerialNumberLogDto
    {
        public int MachineSerialNumberLogId { get; set; }

        public string? SerialNumber { get; set; }

        public string? MachineSerialNumberComponentId { get; set; }

        public int? AccountTriggerId { get; set; }

        public int? AccountTriggerName { get; set; }

        public string? Type { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Action { get; set; }
    }
}
