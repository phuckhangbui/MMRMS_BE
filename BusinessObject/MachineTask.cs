namespace BusinessObject;

public partial class MachineTask
{
    public int MachineTaskId { get; set; }

    public string? TaskTitle { get; set; }

    public string? ContractId { get; set; }

    public int? RequestResponseId { get; set; }

    //public string? ComponentReplacementTicketId { get; set; }

    //public int? PreviousTaskId { get; set; }

    public string? Content { get; set; }

    public int? StaffId { get; set; }

    public int? ManagerId { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateCompleted { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public string? Type { get; set; }

    public string? ConfirmationPictureUrl { get; set; }

    public virtual Account? Staff { get; set; }

    public virtual Account? Manager { get; set; }

    public virtual Contract? Contract { get; set; }

    //public virtual MachineTask? PreviousTask { get; set; }

    public virtual RequestResponse? RequestResponse { get; set; }

    //public virtual ComponentReplacementTicket? ComponentReplacementTicket { get; set; }

    public virtual ICollection<ComponentReplacementTicket> ComponentReplacementTicketsCreateFromTask { get; set; } = new List<ComponentReplacementTicket>();

    public virtual ICollection<MachineTaskLog> MachineTaskLogs { get; set; } = new List<MachineTaskLog>();
}
