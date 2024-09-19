using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Service.Interface;
using Service.Mail;

namespace Service.Implement
{
    public class MailService : IMailService
    {
        private MailSetting _mailSetting { get; set; }
        public MailService(IOptions<MailSetting> options)
        {
            _mailSetting = options.Value;
        }

        public void SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail);
            email.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail));
            email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
            email.Subject = mailContent.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtpClient.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_mailSetting.Mail, _mailSetting.Password);
                smtpClient.Send(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            smtpClient.Disconnect(true);
        }


    }
}
