namespace DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }

        public int? AccountReceiveId { get; set; }

        public int? NotificationType { get; set; }

        public string? MessageNotification { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateRead { get; set; }

        public string? LinkForward { get; set; }

        public string? Status { get; set; }
    }

    public class CreateNotificationDto
    {

        public int? AccountReceiveId { get; set; }

        public int? NotificationType { get; set; }

        public string? MessageNotification { get; set; }

        public string? LinkForward { get; set; }

    }
}
