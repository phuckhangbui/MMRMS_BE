using DTOs.Notification;

namespace Repository.Interface
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsForReceiver(int accountId);
        Task CreateNotification(CreateNotificationDto createNotificationDto);
        Task<bool> UpdateNotificationStatus(int id);
    }
}
