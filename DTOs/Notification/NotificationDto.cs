﻿using Common.Enum;

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

        public string? Status { get; set; }

        public string? DetailId { get; set; }

        public string? DetailIdName { get; set; }

        public static string GetDetailIdName(string notificationType)
        {
            switch (notificationType)
            {
                case var nt when nt == NotificationTypeEnum.Contract.ToString():
                    return "ContractId";

                case var nt when nt == NotificationTypeEnum.Feedback.ToString():
                    return "FeedbackId";

                case var nt when nt == NotificationTypeEnum.Task.ToString():
                    return "MachineTaskId";

                case var nt when nt == NotificationTypeEnum.Invoice.ToString():
                    return "InvoiceId";

                case var nt when nt == NotificationTypeEnum.DeliveryTask.ToString():
                    return "DeliveryTaskId";

                case var nt when nt == NotificationTypeEnum.ComponentReplacementTicket.ToString():
                    return "ComponentReplacementTicketId";

                case var nt when nt == NotificationTypeEnum.MachineCheckRequest.ToString():
                    return "MachineCheckRequestId";

                case var nt when nt == NotificationTypeEnum.RentingRequest.ToString():
                    return "RentingRequestId";

                case var nt when nt == NotificationTypeEnum.Account.ToString():
                    return "AccountId";

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

        public string DetailId { get; set; }

        public string DetailIdName { get; set; }

    }


}
