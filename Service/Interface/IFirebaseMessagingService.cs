namespace Service.Interface
{
    public interface IFirebaseMessagingService
    {
        Task<string> SendPushNotification(string registrationToken, string title, string body, Dictionary<string, string> data = null);
        Task<List<string>> SendPushNotifications(List<string> registrationTokens, string title, string body, Dictionary<string, string> data = null);

    }
}
