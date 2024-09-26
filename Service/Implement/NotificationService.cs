using Common;
using DTOs.Notification;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
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
    }
}
