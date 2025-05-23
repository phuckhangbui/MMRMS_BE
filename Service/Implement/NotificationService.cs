﻿using Common;
using Common.Enum;
using DTOs.Account;
using DTOs.ComponentReplacementTicket;
using DTOs.Contract;
using DTOs.MachineCheckRequest;
using DTOs.MachineTask;
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

        public async Task SendNotificationToManagerWhenTaskStatusUpdated(int managerId, string taskTitle, string status, string detailId)
        {
            string title = "Cập nhật trạng thái công việc";
            string body = $"Trạng thái công việc của {taskTitle} đã được đổi thành [{status}]";


            string type = NotificationTypeEnum.Task.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);

            var account = await _accountRepository.GetAccounById(managerId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = managerId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", managerId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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



        public async Task SendNotificationToStaffWhenTaskStatusUpdated(int staffId, int taskId, string status, string detailId)
        {
            string title = "Cập nhật trạng thái công việc";
            string body = $"Trạng thái công việc số {taskId} đã được đổi thành [{status}]";


            string type = NotificationTypeEnum.Task.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(staffId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = staffId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", staffId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToStaffWhenAssignTaskToCheckMachine(int staffId, ContractAddressDto contractAddress, DateTime dateStart, string detailId)
        {
            string title = "Bạn có thêm một nhiệm vụ kiểm tra máy mới";
            string body = $"Kiểm tra máy tại địa chỉ {contractAddress.AddressBody}, {contractAddress.District} vào ngày {dateStart.Date.ToString(GlobalConstant.DateOnlyFormat)}";


            string type = NotificationTypeEnum.Task.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(staffId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = staffId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", staffId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToStaffWhenDeliveryTaskStatusUpdated(int staffId, ContractAddressDto contractAddress, string status, string detailId)
        {
            string title = "Cập nhật trạng thái giao hàng";
            string body = $"Trạng thái giao hàng tại địa chỉ {contractAddress.AddressBody}, {contractAddress.District} đã được đổi thành [{status}]";


            string type = NotificationTypeEnum.DeliveryTask.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(staffId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = staffId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", staffId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToManagerWhenDeliveryTaskStatusUpdated(int managerId, ContractAddressDto contractAddress, string status, string detailId)
        {
            string title = "Cập nhật trạng thái giao hàng";
            string body = $"Trạng thái giao hàng tại địa chỉ {contractAddress.AddressBody}, {contractAddress.District} đã được đổi thành [{status}]";


            string type = NotificationTypeEnum.DeliveryTask.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(managerId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = managerId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", managerId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToCustomerWhenDeliveryTaskStatusUpdated(int customerId, ContractAddressDto contractAddress, string status, string detailId)
        {
            string title = "Cập nhật trạng thái giao hàng";
            string body = $"Trạng thái giao hàng tại địa chỉ {contractAddress.AddressBody}, {contractAddress.District} đã được đổi thành [{status}]";


            string type = NotificationTypeEnum.DeliveryTask.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(customerId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = customerId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", customerId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToStaffWhenAssignDeliveryTask(int staffId, ContractAddressDto contractAddress, DateTime dateShip, string detailId)
        {
            string title = "Bạn có thêm một nhiệm vụ giao hàng mới";
            string body = $"Giao hàng tại địa chỉ {contractAddress.AddressBody}, {contractAddress.District} vào ngày {dateShip.Date.ToString(GlobalConstant.DateOnlyFormat)}";


            string type = NotificationTypeEnum.DeliveryTask.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(staffId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = staffId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", staffId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToManagerWhenCancelCheckRequest(string machineCheckRequestId, string contractId, ContractAddressDto contractAddress)
        {
            string title = "Yêu cầu kiểm tra máy đã được khách hủy";
            string body = $"Yêu cầu kiểm tra máy của hợp đồng {contractId} tại {contractAddress.AddressBody}, {contractAddress.District} đã được khách hủy";

            var managerList = await _accountRepository.GetManagerAccounts();

            string type = NotificationTypeEnum.MachineCheckRequest.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
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
                        DetailIdName = detailIdName,
                        DetailId = machineCheckRequestId,
                    };

                    var notificationDto = await _notificationRepository.CreateNotification(noti);
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", account.AccountId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

            };
        }

        public async Task SendToManagerWhenCustomerCreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto, string detailId)
        {
            string title = "Yêu cầu kiểm tra máy";
            string body = $"Có một yêu cầu kiểm tra máy của hợp đồng {createMachineCheckRequestDto.ContractId}";

            var managerList = await _accountRepository.GetManagerAccounts();

            string type = NotificationTypeEnum.MachineCheckRequest.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
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
                        DetailIdName = detailIdName,
                        DetailId = detailId,
                    };

                    var notificationDto = await _notificationRepository.CreateNotification(noti);
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", account.AccountId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToStaffWhenAssignTaskToCheckMachineInStorage(int staffId, MachineTaskDto task, DateTime parsedDate, string detailId)
        {
            string title = "Yêu cầu kiểm tra máy";
            string body = $"Có một yêu cầu kiểm tra máy {task.SerialNumber} hiện đang ở trong kho";

            var managerList = await _accountRepository.GetManagerAccounts();

            string type = NotificationTypeEnum.MachineCheckRequest.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
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
                        DetailIdName = detailIdName,
                        DetailId = detailId,
                    };

                    var notificationDto = await _notificationRepository.CreateNotification(noti);
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", account.AccountId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToCustomerWhenCreateComponentReplacementTicket(int customerId, double totalAmount, string componentName, string detailId)
        {
            string title = "Bạn có ticket thay sửa bộ phận cần được thanh toán";
            string body = $"Bộ phận {componentName} cần được thay/sửa với tổng giá tiền là {totalAmount}";


            string type = NotificationTypeEnum.ComponentReplacementTicket.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(customerId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = customerId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", customerId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToStaffWhenCustomerPayTicket(ComponentReplacementTicketDto ticket, string detailId)
        {
            string title = "Một ticket thay thế bộ phận máy của bạn đã được khách thanh toán";
            string body = $"Ticket thay bộ phận {ticket.ComponentName} của máy {ticket.SerialNumber} đã được thanh toán";


            string type = NotificationTypeEnum.ComponentReplacementTicket.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById((int)ticket.EmployeeCreateId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = (int)ticket.EmployeeCreateId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", ((int)ticket.EmployeeCreateId).ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToStaffWhenCustomerCancelTicket(ComponentReplacementTicketDto ticket, string detailId)
        {
            string title = "Một ticket thay thế bộ phận máy của bạn đã bị khách từ chối thanh toán";
            string body = $"Ticket thay bộ phận {ticket.ComponentName} của máy {ticket.SerialNumber} đã được khách hủy";


            string type = NotificationTypeEnum.ComponentReplacementTicket.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById((int)ticket.EmployeeCreateId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = (int)ticket.EmployeeCreateId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", ((int)ticket.EmployeeCreateId).ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToCustomerWhenStaffCancelTicket(ComponentReplacementTicketDto ticket, string componentReplacementTicketId, string? note, int? customerId)
        {
            string title = "Ticket thay thế bộ phận máy đã được nhân viên sửa chữa hủy hủy";
            string body = $"Ticket thay bộ phận {ticket.ComponentName} của máy {ticket.SerialNumber} đã được nhân viên kỹ thuật hủy với lý do {note}";


            string type = NotificationTypeEnum.ComponentReplacementTicket.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById((int)customerId);

            if (account == null)
            {
                return;
            }

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = (int)ticket.EmployeeCreateId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = ticket.ComponentReplacementTicketId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", ((int)ticket.EmployeeCreateId).ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToCustomerWhenUpdateRequestStatus(int accountSignId, MachineCheckRequestDto request, string detailId)
        {
            string title = $"Yêu cầu kiểm tra máy của bạn đã được thay đổi trạng thái";
            string body = $"Yêu cầu số {request.MachineCheckRequestId} của máy {request.SerialNumber} đã được đổi thành [{EnumExtensions.TranslateStatus<MachineCheckRequestStatusEnum>(request.Status.ToString())}]";


            string type = NotificationTypeEnum.MachineCheckRequest.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(accountSignId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = accountSignId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", accountSignId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToManagerWhenCustomerSignedAllContract(string customerName, string rentingRequestId, string detailId)
        {
            string title = "Khách hàng đã ký tất cả hợp đồng";
            string body = $"Khách hàng {customerName} đã ký tất cả hợp đồng cho yêu cầu thuê {rentingRequestId}.";

            var managerList = await _accountRepository.GetManagerAccounts();
            string type = NotificationTypeEnum.RentingRequest.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);

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
                        DetailIdName = detailIdName,
                        DetailId = detailId,
                    };

                    var notificationDto = await _notificationRepository.CreateNotification(noti);
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type },
                        { "accountId", account.AccountId.ToString() },
                        { "detailIdName", noti.DetailIdName },
                        { "detailId", noti.DetailId },
                        { "notificationId", notificationDto.NotificationId.ToString() }
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

        public async Task SendNotificationToManagerWhenCustomerEndContract(string contractId, string detailId)
        {
            string title = "Khách hàng đã kết thúc hợp đồng";
            string body = $"Khách hàng đã kết thúc hợp đồng {contractId}.";

            var managerList = await _accountRepository.GetManagerAccounts();
            string type = NotificationTypeEnum.Contract.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);

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
                        DetailIdName = detailIdName,
                        DetailId = detailId,
                    };

                    var notificationDto = await _notificationRepository.CreateNotification(noti);
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type },
                        { "accountId", account.AccountId.ToString() },
                        { "detailIdName", noti.DetailIdName },
                        { "detailId", noti.DetailId },
                        { "notificationId", notificationDto.NotificationId.ToString() }
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

        public async Task SendNotificationToManagerWhenCustomerExtendContract(string contractId, string detailId)
        {
            string title = "Khách hàng đã gia hạn hợp đồng";
            string body = $"Khách hàng đã gia hạn hợp đồng {contractId}.";

            var managerList = await _accountRepository.GetManagerAccounts();
            string type = NotificationTypeEnum.Contract.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);

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
                        DetailIdName = detailIdName,
                        DetailId = detailId,
                    };

                    var notificationDto = await _notificationRepository.CreateNotification(noti);
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type },
                        { "accountId", account.AccountId.ToString() },
                        { "detailIdName", noti.DetailIdName },
                        { "detailId", noti.DetailId },
                        { "notificationId", notificationDto.NotificationId.ToString() }
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

        public async Task SendNotificationToCustomerWhenManagerCreateRefundInvoice(string invoiceType, int customerId, string contractId, string detailId)
        {
            string title = "";
            string body = "";
            if (invoiceType != null && invoiceType.Equals(InvoiceTypeEnum.Refund.ToString()))
            {
                title = "Hoá đơn hoàn tiền đã được tạo";
                body = $"Hoá đơn hoàn tiền cho hợp đồng {contractId} đã được tạo.";
            }
            else if (invoiceType != null && invoiceType.Equals(InvoiceTypeEnum.DamagePenalty.ToString()))
            {
                title = "Hoá đơn tiền bồi thường đã được tạo";
                body = $"Hoá đơn tiền bồi thường cho hợp đồng {contractId} đã được tạo.";
            }

            string type = NotificationTypeEnum.Invoice.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(customerId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = customerId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", customerId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToCustomerWhenLatePayment(int customerId, string contractId, DateTime dateFrom, string detailId)
        {
            string title = "Thanh toán sắp đến hạn!";
            string body = $"Hóa đơn cho hợp đồng {contractId} sắp đến hạn thanh toán vào ngày {dateFrom}. Vui lòng thanh toán đúng hạn để tránh bị phạt.";

            string type = NotificationTypeEnum.Invoice.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(customerId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = customerId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", customerId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

        public async Task SendNotificationToManagersWhenNewCustomerNeedConfirmation(AccountDto accountDto)
        {
            string title = "Có một tài khoản khách mới cần được duyệt";
            string body = $"Một tài khoản khách của người dùng tên ${accountDto?.Name} vừa được tạo, hãy bắt đầu quá trình kiểm tra tài khoản";

            var managerList = await _accountRepository.GetManagerAccounts();

            string type = NotificationTypeEnum.Account.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
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
                        DetailIdName = detailIdName,
                        DetailId = accountDto.AccountId.ToString(),
                    };

                    var notificationDto = await _notificationRepository.CreateNotification(noti);
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", account.AccountId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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

            };
        }

        public async Task SendNotificationToCustomerWhenSystemCancelRentingRequest(int customerId, string rentingRequestId, string detailId)
        {
            string title = "Yêu cầu thuê máy của bạn đã bị hủy";
            string body = "Yêu cầu thuê máy với mã đơn '" + rentingRequestId + "' đã quá hạn xử lý và bị hủy tự động.";

            string type = NotificationTypeEnum.RentingRequest.ToString();
            string detailIdName = NotificationDto.GetDetailIdName(type);
            var account = await _accountRepository.GetAccounById(customerId);

            try
            {
                var noti = new CreateNotificationDto
                {
                    AccountReceiveId = customerId,
                    NotificationTitle = title,
                    MessageNotification = body,
                    NotificationType = type,
                    DetailIdName = detailIdName,
                    DetailId = detailId,
                };

                var notificationDto = await _notificationRepository.CreateNotification(noti);
                Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "type", type.ToString() },
                        { "accountId", customerId.ToString() },
                        { "detailIdName", noti.DetailIdName},
                        { "detailId", noti.DetailId},
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
    }
}
