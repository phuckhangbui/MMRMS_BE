using Common;
using Common.Enum;
using DTOs.MaintenanceRequest;
using DTOs.Notification;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IFirebaseMessagingService _messagingService;

        public NotificationService(INotificationRepository notificationRepository, IAccountRepository accountRepository, IFirebaseMessagingService messagingService)
        {
            _notificationRepository = notificationRepository;
            _accountRepository = accountRepository;
            _messagingService = messagingService;
        }

        public async Task CreateNotification(CreateNotificationDto createNotificationDto)
        {
            await _notificationRepository.CreateNotification(createNotificationDto);
        }

        public Task<IEnumerable<NotificationDto>> GetNotificationsBaseOnReceiveId(int accountId)
        {
            return _notificationRepository.GetNotificationsForReceiver(accountId);

        }

        public async Task MarkNotificationAsRead(int id)
        {
            var isTaskSuccess = await _notificationRepository.UpdateNotificationStatus(id);

            if (!isTaskSuccess)
            {
                throw new Exception(MessageConstant.Notification.NotificationNotFound);
            }


        }

        public async Task SendToManagerWhenCustomerCreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto)
        {
            string title = "Yêu cầu kiểm tra máy";
            string body = $"Có một yêu cầu kiểm tra ở máy {createMaintenanceRequestDto.SerialNumber} của hợp đồng {createMaintenanceRequestDto.ContractId}";

            var managerList = await _accountRepository.GetManagerAccounts();

            string type = NotificationTypeEnum.RequestMaintenance.ToString();
            string linkForward = NotificationDto.GetForwardPath(type);

            if (managerList.IsNullOrEmpty())
            {
                return;
            }
            try
            {
                foreach (var account in managerList)
                {
                    var noti = new CreateNotificationDto
                    {
                        AccountReceiveId = account.AccountId,
                        NotificationTitle = title,
                        MessageNotification = body,
                        NotificationType = type,
                        LinkForward = linkForward,
                    };

                    var notificationDto = await _notificationRepository.CreateNotification(noti);
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", account.AccountId.ToString() },
                        { "forwardToPath", noti.LinkForward },
                        {"notificationId", notificationDto.NotificationId.ToString() }
                    };

                    if (!account.FirebaseMessageToken.IsNullOrEmpty())
                    {
                        _messagingService.SendPushNotification(account.FirebaseMessageToken, title, body, data);
                    }
                }

            }
            catch (Exception ex)
            {

            }


        }
    }
}
