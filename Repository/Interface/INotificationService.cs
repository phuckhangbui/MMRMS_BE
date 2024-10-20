using DTOs.Contract;
using DTOs.MachineCheckRequest;
using DTOs.Notification;

namespace Repository.Interface
{
    public interface INotificationService
    {
        Task CreateNotification(CreateNotificationDto createNotificationDto);
        Task MarkNotificationAsRead(int id);
        Task<IEnumerable<NotificationDto>> GetNotificationsBaseOnReceiveId(int accountId);
        Task SendToManagerWhenCustomerCreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto);
        Task SendNotificationToManagerWhenTaskStatusUpdated(int managerId, string taskTitle, string status);
        Task SendNotificationToStaffWhenTaskStatusUpdated(int staffId, string taskTitle, string status);
        Task SendNotificationToStaffWhenDeliveryTaskStatusUpdated(int staffId, ContractAddressDto contractAddress, string status);
        Task SendNotificationToStaffWhenAssignDeliveryTask(int staffId, ContractAddressDto contractAddress, DateTime dateShip);
        Task SendNotificationToStaffWhenAssignTaskToCheckMachine(int staffId, ContractAddressDto contractAddress, DateTime dateStart);
        Task SendNotificationToCustomerWhenCreateComponentReplacementTicket(int customerId, double totalAmount, string? componentName);
    }
}
