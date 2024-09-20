using DTOs;
using DTOs.Authentication;

namespace Service.Interface
{
    public interface IAuthenticationService
    {
        Task<LoginAccountDto> Login(LoginUsernameDto loginUsernameDto);
        Task<LoginAccountDto> Login(LoginEmailDto loginEmail);
        Task<LoginAccountDto> ForgetPassword(string email);
        Task<LoginAccountDto> RefreshToken(TokenApiDto tokenApiDto);
        Task Logout(int accountId);
    }
}
