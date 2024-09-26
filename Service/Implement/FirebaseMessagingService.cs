using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Service.Interface;

namespace Service.Implement
{
    public class FirebaseMessagingService : IFirebaseMessagingService
    {
        private readonly FirebaseApp _firebaseApp;

        public FirebaseMessagingService(string jsonCredentialsPath)
        {
            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(jsonCredentialsPath)
            });
        }

        public async Task<string> SendPushNotification(string registrationToken, string title, string body, Dictionary<string, string> data = null)
        {
            var message = new Message()
            {
                Data = data ?? new Dictionary<string, string>(),
                Token = registrationToken,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("Successfully sent message: " + response);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message: " + ex.Message);
            }
            return $"Send message for {registrationToken} fail";
        }

        public async Task<List<string>> SendPushNotifications(List<string> registrationTokens, string title, string body, Dictionary<string, string> data = null)
        {
            var responses = new List<string>();

            foreach (var token in registrationTokens)
            {
                try
                {
                    string response = await SendPushNotification(token, title, body, data);
                    responses.Add(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to token {token}: {ex.Message}");
                    responses.Add($"Error sending message to token {token}: {ex.Message}");
                }
            }

            return responses;
        }
    }
}
