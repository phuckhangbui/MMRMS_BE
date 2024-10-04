using Common.Enum;

namespace DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }

        public int? AccountReceiveId { get; set; }

        public string? NotificationType { get; set; }

        public string? NotificationTitle { get; set; }

        public string? MessageNotification { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateRead { get; set; }

        public string? LinkForward { get; set; }

        public string? Status { get; set; }

        public static string GetForwardPath(string notificationType)
        {
            switch (notificationType)
            {
                case var nt when nt == NotificationTypeEnum.Contract.ToString():
                    return "/contracts";

                case var nt when nt == NotificationTypeEnum.Feedback.ToString():
                    return "/feedback";

                case var nt when nt == NotificationTypeEnum.Task.ToString():
                    return "/tasks";

                case var nt when nt == NotificationTypeEnum.Billing.ToString():
                    return "/billing";

                case var nt when nt == NotificationTypeEnum.Delivery.ToString():
                    return "/delivery";

                case var nt when nt == NotificationTypeEnum.RequestMaintenance.ToString():
                    return "/maintenance-requests";

                case var nt when nt == NotificationTypeEnum.MaintenanceTicket.ToString():
                    return "/maintenance-tickets";

                default:
                    return null;
            }
        }
    }

    public class CreateNotificationDto
    {

        public int AccountReceiveId { get; set; }

        public string NotificationTitle { get; set; }

        public string NotificationType { get; set; }

        public string MessageNotification { get; set; }

        public string LinkForward { get; set; }

    }


}
