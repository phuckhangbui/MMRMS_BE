namespace BusinessObject
{
    public partial class MachineSerialNumberLog
    {
        public int MachineSerialNumberLogId { get; set; }

        public string? SerialNumber { get; set; }

        public string? MachineSerialNumberComponentId { get; set; }

        public int? AccountTriggerId { get; set; }

        public string? Type { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Action { get; set; }

        public virtual MachineSerialNumber? MachineSerialNumber { get; set; }

        public virtual Account? AccountTrigger { get; set; }
    }
}
