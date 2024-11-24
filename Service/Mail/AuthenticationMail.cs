namespace Service.Mail
{
    public class AuthenticationMail
    {
        public static MailContent SendWelcomeAndOtpToCustomer(string toEmail, string name, string otp)
        {
            var mailContext = new MailContent();
            mailContext.To = toEmail;
            mailContext.Subject = $"Chào mừng đến với MMRMS - Hệ thống Quản lý Thuê Máy Móc";

            mailContext.Body = $@"
    <p>Kính gửi Quý khách hàng <b>{name}</b>,</p>

    <p>Chúng tôi xin chân thành cảm ơn Quý khách đã tin tưởng và lựa chọn MMRMS - Hệ thống Quản lý Thuê Máy Móc của chúng tôi.</p>
    
    <p>Để xác nhận đăng ký tài khoản của Quý khách, vui lòng nhập mã OTP sau:</p>
    <p><b>{otp}</b></p>
    
    <p>Vui lòng không chia sẻ mã OTP này với bất kỳ ai để bảo vệ tài khoản của Quý khách.</p>
    
    <p>Trân trọng,<br>
    Đội ngũ hỗ trợ MMRMS - Hệ thống Quản lý Thuê Máy Móc</p>";

            return mailContext;
        }


        public static MailContent SendOtpToCustomer(string toEmail, string name, string otp)
        {
            var mailContext = new MailContent();
            mailContext.To = toEmail;
            mailContext.Subject = $"Mã OTP của Quý khách từ MMRMS";

            mailContext.Body = $@"
    <p>Kính gửi Quý khách hàng <b>{name}</b>,</p>

    <p>Chúng tôi xin gửi mã OTP của Quý khách để thực hiện giao dịch trên hệ thống:</p>
    <p><b>{otp}</b></p>
    
    <p>Vui lòng không chia sẻ mã OTP này với bất kỳ ai để đảm bảo an toàn thông tin.</p>
    
    <p>Chúc Quý khách có trải nghiệm tuyệt vời khi sử dụng dịch vụ của chúng tôi.</p>
    
    <p>Trân trọng,<br>
    Đội ngũ hỗ trợ MMRMS - Hệ thống Quản lý Thuê Máy Móc</p>";

            return mailContext;
        }


        public static MailContent SendWelcomeAndCredentialsToEmployee(string toEmail, string name, string username, string password)
        {
            var mailContext = new MailContent();
            mailContext.To = toEmail;
            mailContext.Subject = $"Chào mừng đến với MMRMS - Hệ thống Quản lý Thuê Máy Móc";

            mailContext.Body = $@"
    <p>Kính gửi anh/chị <b>{name}</b>,</p>

    <p>Chào mừng anh/chị đã trở thành thành viên của đội ngũ MMRMS - Hệ thống Quản lý Thuê Máy Móc.</p>

    <p>Tài khoản của anh/chị đã được tạo thành công trên hệ thống. Dưới đây là thông tin tài khoản:</p>
    <ul>
        <li><b>Tên đăng nhập:</b> {username}</li>
        <li><b>Mật khẩu:</b> {password}</li>
    </ul>

    <p>Vui lòng đăng nhập và thay đổi mật khẩu ngay sau lần sử dụng đầu tiên để đảm bảo an toàn thông tin.</p>
    
    <p>Nếu có bất kỳ thắc mắc nào, anh/chị vui lòng liên hệ bộ phận hỗ trợ qua email hoặc số điện thoại.</p>

    <p>Trân trọng,<br>
    Đội ngũ MMRMS - Hệ thống Quản lý Thuê Máy Móc</p>";

            return mailContext;
        }

    }
}
