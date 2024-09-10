using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? AccountReceiveId { get; set; }

    public int? NotificationType { get; set; }

    public string? MessageNotification { get; set; }

    public string? LinkForward { get; set; }

    public string? Status { get; set; }

    public virtual Account? AccountReceive { get; set; }
}
