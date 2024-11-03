using DTOs.Account;
using DTOs.Authentication;

namespace Service.Interface
{
    public interface IAuthenticationService
    {
        Task<LoginAccountDto> LoginUsername(LoginUsernameDto loginUsernameDto);
        Task<LoginAccountDto> LoginEmail(LoginEmailDto loginEmail);
        Task<LoginAccountDto> ForgetPassword(string email);
        Task<LoginAccountDto> RefreshToken(TokenApiDto tokenApiDto);
        Task Logout(int accountId);
        Task RegisterCustomer(NewCustomerAccountDto newCustomerAccountDto);
        Task ActivateMemberEmailByOtp(MemberConfirmEmailOtpDto memberConfirmEmailOtpDto);
        Task SendOtp(string email);
        Task ConfirmOtpAndChangePasswordWhenForget(ChangePasswordWithOtpDto changePasswordWithOtp);
        Task ChangePasswordWithOldPassword(int accountId, ChangePasswordDto changePasswordDto);


    }
}
