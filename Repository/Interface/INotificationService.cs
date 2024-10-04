using DTOs.MaintenanceRequest;
using DTOs.Notification;

namespace Service.Interface
{
    public interface INotificationService
    {
        Task CreateNotification(CreateNotificationDto createNotificationDto);
        Task MarkNotificationAsRead(int id);
        Task<IEnumerable<NotificationDto>> GetNotificationsBaseOnReceiveId(int accountId);
        Task SendToManagerWhenCustomerCreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto);
    }
}
