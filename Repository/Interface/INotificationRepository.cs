using DTOs.Notification;

namespace Repository.Interface
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsForReceiver(int accountId);
        Task<NotificationDto> CreateNotification(CreateNotificationDto createNotificationDto);
        Task<bool> UpdateNotificationStatus(int id);
    }
}
