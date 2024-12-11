
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
    Đội ngũ hỗ trợ <b>MMRMS</b> - Hệ thống Quản lý Thuê Máy Móc</p>";

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
    Đội ngũ hỗ trợ <b>MMRMS</b> - Hệ thống Quản lý Thuê Máy Móc</p>";

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
    Đội ngũ hỗ trợ <b>MMRMS</b> - Hệ thống Quản lý Thuê Máy Móc</p>";

            return mailContext;
        }

        public static MailContent SendApprovalEmailToCustomer(string email, string name)
        {
            var mailContext = new MailContent();
            mailContext.To = email;
            mailContext.Subject = $"Tài khoản của bạn đã được phê duyệt - MMRMS";

            mailContext.Body = $@"
    <p>Kính gửi <b>{name}</b>,</p>

    <p>Chúng tôi xin trân trọng thông báo rằng tài khoản của bạn trên <b>MMRMS</b> - Hệ thống Quản lý Thuê Máy Móc đã được phê duyệt thành công sau khi kiểm tra hồ sơ. Bạn có thể sử dụng tài khoản để truy cập vào hệ thống và khám phá các tính năng hữu ích của chúng tôi.</p>

    <p>Với tài khoản đã được phê duyệt, bạn có thể:</p>
    <ul>
        <li>Gửi yêu cầu thuê máy móc nhanh chóng và thuận tiện.</li>
        <li>Quản lý các hợp đồng thuê và tiến trình thanh toán một cách dễ dàng.</li>
        <li>Tạo các yêu cầu kiểm tra máy trong quá trình thuê.</li>
    </ul>

    <p>Chúng tôi mong muốn mang đến cho bạn trải nghiệm dịch vụ chuyên nghiệp và tiện lợi nhất</p>

    <p>Trân trọng,<br>
    Đội ngũ hỗ trợ <b>MMRMS</b> - Hệ thống Quản lý Thuê Máy Móc</p>";

            return mailContext;
        }


        public static MailContent SendDisapprovalEmailToCustomer(string email, string name, string note)
        {
            var mailContext = new MailContent();
            mailContext.To = email;
            mailContext.Subject = $"Yêu cầu phê duyệt tài khoản bị từ chối - MMRMS";

            mailContext.Body = $@"
    <p>Kính gửi <b>{name}</b>,</p>

    <p>Chúng tôi xin thông báo rằng tài khoản của bạn trên <b>MMRMS</b> - Hệ thống Quản lý Thuê Máy Móc không được phê duyệt với lý do sau:</p>

    <p><i>{note}</i></p>

    <p>Chúng tôi hiểu rằng điều này có thể gây bất tiện, và chúng tôi khuyến khích bạn kiểm tra lại các thông tin bạn đã cung cấp. Bạn vẫn có thể tiếp tục sử dụng email này để đăng kí vào hệ thống chúng tôi sau này.</p>
    <p>Chúng tôi rất mong được hỗ trợ bạn trong thời gian tới!</p>

    <p>Trân trọng,<br>
    Đội ngũ hỗ trợ <b>MMRMS</b> - Hệ thống Quản lý Thuê Máy Móc</p>";

            return mailContext;
        }

    }
}
