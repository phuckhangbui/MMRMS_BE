namespace Service.Mail
{
    public class AuthenticationMail
    {
        public static MailContent SendWelcomeAndOtpToCustomer(string toEmail, string name, string otp)
        {
            var mailContext = new MailContent();
            mailContext.To = toEmail;
            mailContext.Subject = $"Welcome to MMRMS - Mechanical Machinery Rental Management System";
            mailContext.Body = $@"<p>Dear {name}</p>
                                    <p>Here is your opt: {otp}</p>
                                    <p>MMRMS - Mechanical Machinery Rental Management System</p>";
            return mailContext;
        }

        public static MailContent SendOtpToCustomer(string toEmail, string name, string otp)
        {
            var mailContext = new MailContent();
            mailContext.To = toEmail;
            mailContext.Subject = $"Your MMRMS OTP is here";
            mailContext.Body = $@"<p>Dear {name}</p>
                                    <p>Here is your opt: {otp}</p>
                                    <p>MMRMS - Mechanical Machinery Rental Management System</p>";
            return mailContext;
        }
    }
}
