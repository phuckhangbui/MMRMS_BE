using AutoMapper;
using BusinessObject;
using DAO;
using DAO.Enum;
using DTOs.Notification;
using Repository.Interface;

namespace Repository.Implement
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMapper _mapper;

        public NotificationRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreateNotification(CreateNotificationDto createNotificationDto)
        {
            var notification = _mapper.Map<Notification>(createNotificationDto);

            notification.DateCreate = DateTime.Now;
            notification.Status = NotificationStatusEnum.Send.ToString();

            await NotificationDao.Instance.CreateAsync(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsForReceiver(int accountId)
        {
            var notifications = await NotificationDao.Instance.GetNotificationByReceiverId(accountId);

            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task<bool> UpdateNotificationStatus(int id)
        {
            var notification = await NotificationDao.Instance.GetNotificationById(id);

            if (notification == null)
            {
                return false;
            }

            notification.Status = NotificationStatusEnum.Read.ToString();
            notification.DateRead = DateTime.Now;

            await NotificationDao.Instance.UpdateAsync(notification);
            return true;
        }
    }
}
