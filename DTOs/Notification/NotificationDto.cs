namespace DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }

        public int? AccountReceiveId { get; set; }

        public string? NotificationType { get; set; }

        public string? MessageNotification { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateRead { get; set; }

        public string? LinkForward { get; set; }

        public string? Status { get; set; }

        public static string GetForwardPath(int notificationType)
        {
            switch (notificationType)
            {
                //case NotificationTypeEnum.Contract:
                //    return "/contracts";

                //case NotificationTypeEnum.owner_create_bill:
                //    return "/payments";

                //case NotificationTypeEnum.member_sign_contract:
                //    return "/owner/contracts";

                //case NotificationTypeEnum.member_pay_bill:
                //    return "/owner/payments";

                //case NotificationTypeEnum.member_make_appointment:
                //    return "/owner/appointments";

                //case NotificationTypeEnum.member_rent_room:
                //    return "/owner/contracts";

                //case NotificationTypeEnum.member_send_complain:
                //    return "/owner/complains";

                //case NotificationTypeEnum.owner_reply_complain:
                //    return "/complains";

                //case NotificationTypeEnum.owner_package_expire:
                //    return "/owner/packages";

                //case NotificationTypeEnum.contract_finish:
                //    return "/contracts";

                //case NotificationTypeEnum.owner_create_contract_but_this_is_for_decline_member:
                //    return "/member/contracts";

                //case NotificationTypeEnum.member_decline_contract:
                //    return null;

                //case NotificationTypeEnum.member_make_hiring_request:
                //    return "/owner/appointments";

                default:
                    return null;
            }
        }
    }

    public class CreateNotificationDto
    {

        public int? AccountReceiveId { get; set; }

        public int? NotificationType { get; set; }

        public string? MessageNotification { get; set; }

        public string? LinkForward { get; set; }

    }


}
