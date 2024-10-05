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

        public static MailContent SendWelcomeAndCredentialsToEmployee(string toEmail, string name, string username, string password)
        {
            var mailContext = new MailContent();
            mailContext.To = toEmail;
            mailContext.Subject = $"Chào mừng đến với MMRMS - Hệ thống Quản lý Thuê Máy Móc";
            mailContext.Body = $@"<p>Xin chào {name},</p>
                                  <p>Tài khoản của bạn đã được tạo thành công trên hệ thống.</p>
                                  <p>Tên đăng nhập của bạn: {username}</p>
                                  <p>Mật khẩu của bạn: {password}</p>
                                  <p>MMRMS - Hệ thống Quản lý Thuê Máy Móc</p>";
            return mailContext;
        }
    }
}
