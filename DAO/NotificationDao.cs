using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class NotificationDao : BaseDao<Notification>
    {
        private static NotificationDao instance = null;
        private static readonly object instacelock = new object();

        private NotificationDao()
        {

        }

        public static NotificationDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotificationDao();
                }
                return instance;
            }
        }

        public async Task<Notification> GetNotificationById(int notificationId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Notifications.FirstOrDefaultAsync(p => p.NotificationId == notificationId);
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationByReceiverId(int receiverId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Notifications.Where(p => p.AccountReceiveId == receiverId).ToListAsync();
            }
        }
    }
}
