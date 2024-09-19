
using Service.Mail;

namespace Service.Interface
{
    public interface IMailService
    {
        void SendMail(MailContent mailContent);
    }
}
