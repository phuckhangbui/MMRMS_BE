namespace BusinessObject;

public partial class MachineTaskLog
{
    public int MachineTaskLogId { get; set; }

    public int? MachineTaskId { get; set; }

    public int? AccountTriggerId { get; set; }

    public string? Action { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual MachineTask? MachineTask { get; set; }

    public virtual Account? AccountTrigger { get; set; }
}
