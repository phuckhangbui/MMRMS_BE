using DTOs.Contract;
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
        Task SendNotificationToManagerWhenTaskStatusUpdated(int managerId, string taskTitle, string status);
        Task SendNotificationToStaffWhenTaskStatusUpdated(int staffId, string taskTitle, string status);
        Task SendNotificationToStaffWhenDeliveryStatusUpdated(int staffId, ContractAddressDto contractAddress, string status);
        Task SendNotificationToStaffWhenAssignDelivery(int staffId, ContractAddressDto contractAddress, DateTime dateShip);
        Task SendNotificationToStaffWhenAssignTaskToCheckMachine(int staffId, ContractAddressDto contractAddress, DateTime dateStart);
        Task SendNotificationToCustomerWhenCreateMaintenanceTicket(int customerId, double totalAmount, string? componentName);
    }
}
