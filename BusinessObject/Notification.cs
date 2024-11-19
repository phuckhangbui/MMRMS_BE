namespace BusinessObject;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? AccountReceiveId { get; set; }

    public string? NotificationType { get; set; }

    public string? NotificationTitle { get; set; }

    public string? MessageNotification { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateRead { get; set; }

    public string? DetailId { get; set; }

    public string? DetailIdName { get; set; }

    public virtual Account? AccountReceive { get; set; }
}
