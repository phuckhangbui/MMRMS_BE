﻿using DTOs.ComponentReplacementTicket;
using DTOs.Contract;
using DTOs.MachineCheckRequest;
using DTOs.MachineTask;
using DTOs.Notification;

namespace Repository.Interface
{
    public interface INotificationService
    {
        Task CreateNotification(CreateNotificationDto createNotificationDto);
        Task MarkNotificationAsRead(int id);
        Task<IEnumerable<NotificationDto>> GetNotificationsBaseOnReceiveId(int accountId);
        Task SendToManagerWhenCustomerCreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto, string detailId);
        Task SendNotificationToManagerWhenTaskStatusUpdated(int managerId, string taskTitle, string status, string detailId);
        Task SendNotificationToStaffWhenTaskStatusUpdated(int staffId, int taskId, string status, string detailId);
        Task SendNotificationToStaffWhenDeliveryTaskStatusUpdated(int staffId, ContractAddressDto contractAddress, string status, string detailId);
        Task SendNotificationToManagerWhenDeliveryTaskStatusUpdated(int managerId, ContractAddressDto contractAddress, string status, string detailId);
        Task SendNotificationToStaffWhenAssignDeliveryTask(int staffId, ContractAddressDto contractAddress, DateTime dateShip, string detailId);
        Task SendNotificationToStaffWhenAssignTaskToCheckMachine(int staffId, ContractAddressDto contractAddress, DateTime dateStart, string detailId);
        Task SendNotificationToCustomerWhenCreateComponentReplacementTicket(int customerId, double totalAmount, string? componentName, string detailId);
        Task SendNotificationToStaffWhenCustomerPayTicket(ComponentReplacementTicketDto ticket, string detailId);
        Task SendNotificationToStaffWhenCustomerCancelTicket(ComponentReplacementTicketDto ticket, string detailId);
        Task SendNotificationToCustomerWhenUpdateRequestStatus(int accountSignId, MachineCheckRequestDto request, string detailId);
        Task SendNotificationToStaffWhenAssignTaskToCheckMachineInStorage(int staffId, MachineTaskDto task, DateTime parsedDate, string detailId);
    }
}
