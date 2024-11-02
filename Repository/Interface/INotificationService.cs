using DTOs.ComponentReplacementTicket;
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
        Task SendNotificationToStaffWhenTaskStatusUpdated(int staffId, int taskId, string status);
        Task SendNotificationToStaffWhenDeliveryTaskStatusUpdated(int staffId, ContractAddressDto contractAddress, string status);
        Task SendNotificationToManagerWhenDeliveryTaskStatusUpdated(int managerId, ContractAddressDto contractAddress, string status);
        Task SendNotificationToStaffWhenAssignDeliveryTask(int staffId, ContractAddressDto contractAddress, DateTime dateShip);
        Task SendNotificationToStaffWhenAssignTaskToCheckMachine(int staffId, ContractAddressDto contractAddress, DateTime dateStart);
        Task SendNotificationToCustomerWhenCreateComponentReplacementTicket(int customerId, double totalAmount, string? componentName);
        Task SendNotificationToStaffWhenCustomerPayTicket(ComponentReplacementTicketDto ticket);
        Task SendNotificationToStaffWhenCustomerCancelTicket(ComponentReplacementTicketDto ticket);
        Task SendNotificationToCustomerWhenUpdateRequestStatus(int accountSignId, MachineCheckRequestDto request);
    }
}
