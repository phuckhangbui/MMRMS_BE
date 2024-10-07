﻿using Common;
using Common.Enum;
using DTOs.Contract;
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

        public async Task SendNotificationToManagerWhenTaskStatusUpdated(int managerId, string taskTitle, string status)
        {
            string title = "Cập nhật trạng thái công việc";
            string body = $"Trạng thái công việc của {taskTitle} đã được đổi thành [{status}]";


            string type = NotificationTypeEnum.Task.ToString();
            string linkForward = NotificationDto.GetForwardPath(type);

            var account = await _accountRepository.GetAccounById(managerId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = managerId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    LinkForward = linkForward,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", managerId.ToString() },
                        { "forwardToPath", noti.LinkForward },
                        {"notificationId", notificationDto.NotificationId.ToString() }
                    };

                if (!account.FirebaseMessageToken.IsNullOrEmpty())
                {
                    _messagingService.SendPushNotification(account.FirebaseMessageToken, title, body, data);
                }
            }
            catch
            {

            }
        }



        public async Task SendNotificationToStaffWhenTaskStatusUpdated(int staffId, string taskTitle, string status)
        {
            string title = "Cập nhật trạng thái công việc";
            string body = $"Trạng thái công việc của {taskTitle} đã được đổi thành [{status}]";


            string type = NotificationTypeEnum.Task.ToString();
            string linkForward = NotificationDto.GetForwardPath(type);

            var account = await _accountRepository.GetAccounById(staffId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = staffId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    LinkForward = linkForward,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", staffId.ToString() },
                        { "forwardToPath", noti.LinkForward },
                        {"notificationId", notificationDto.NotificationId.ToString() }
                    };

                if (!account.FirebaseMessageToken.IsNullOrEmpty())
                {
                    _messagingService.SendPushNotification(account.FirebaseMessageToken, title, body, data);
                }
            }
            catch
            {

            }
        }

        public async Task SendNotificationToStaffWhenAssignTaskToMaintenance(int staffId, ContractAddressDto? contractAddress, DateTime dateStart)
        {
            string title = "Bạn có thêm một nhiệm vụ kiểm tra máy vào bảo trì mới";
            string body = $"Kiểm tra máy tại địa chỉ {contractAddress.AddressBody}, {contractAddress.District} vào ngày {dateStart.Date}";


            string type = NotificationTypeEnum.Task.ToString();
            string linkForward = NotificationDto.GetForwardPath(type);

            var account = await _accountRepository.GetAccounById(staffId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = staffId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    LinkForward = linkForward,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", staffId.ToString() },
                        { "forwardToPath", noti.LinkForward },
                        {"notificationId", notificationDto.NotificationId.ToString() }
                    };

                if (!account.FirebaseMessageToken.IsNullOrEmpty())
                {
                    _messagingService.SendPushNotification(account.FirebaseMessageToken, title, body, data);
                }
            }
            catch
            {

            }
        }

        public async Task SendNotificationToStaffWhenDeliveryStatusUpdated(int staffId, ContractAddressDto contractAddress, string status)
        {
            string title = "Cập nhật trạng thái giao hàng";
            string body = $"Trạng thái giao hàng tại địa chỉ {contractAddress.AddressBody}, {contractAddress.District} đã được đổi thành [{status}]";


            string type = NotificationTypeEnum.Delivery.ToString();
            string linkForward = NotificationDto.GetForwardPath(type);

            var account = await _accountRepository.GetAccounById(staffId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = staffId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    LinkForward = linkForward,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", staffId.ToString() },
                        { "forwardToPath", noti.LinkForward },
                        {"notificationId", notificationDto.NotificationId.ToString() }
                    };

                if (!account.FirebaseMessageToken.IsNullOrEmpty())
                {
                    _messagingService.SendPushNotification(account.FirebaseMessageToken, title, body, data);
                }
            }
            catch
            {

            }
        }

        public async Task SendNotificationToStaffWhenAssignDelivery(int staffId, ContractAddressDto? contractAddress, DateTime dateShip)
        {
            string title = "Bạn có thêm một nhiệm vụ giao hàng mới";
            string body = $"Giao hàng tại địa chỉ {contractAddress.AddressBody}, {contractAddress.District} vào ngày {dateShip.Date}";


            string type = NotificationTypeEnum.Delivery.ToString();
            string linkForward = NotificationDto.GetForwardPath(type);

            var account = await _accountRepository.GetAccounById(staffId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = staffId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    LinkForward = linkForward,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", staffId.ToString() },
                        { "forwardToPath", noti.LinkForward },
                        {"notificationId", notificationDto.NotificationId.ToString() }
                    };

                if (!account.FirebaseMessageToken.IsNullOrEmpty())
                {
                    _messagingService.SendPushNotification(account.FirebaseMessageToken, title, body, data);
                }
            }
            catch
            {

            }
        }

        public async Task SendToManagerWhenCustomerCreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto)
        {
            string title = "Yêu cầu kiểm tra máy";
            string body = $"Có một yêu cầu kiểm tra máy của hợp đồng {createMaintenanceRequestDto.ContractId}";

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
