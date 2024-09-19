using DTOs;
using DTOs.Account;
using DTOs.Authentication;

namespace Service.Interface
{
    public interface IAuthenticationService
    {
        Task<AccountBaseDto> Login(LoginUsernameDto loginUsernameDto);
        Task<AccountBaseDto> Login(LoginEmailDto loginEmail);
        Task<AccountBaseDto> ForgetPassword(string email);
        Task<AccountBaseDto> RefreshToken(TokenApiDto tokenApiDto);
        Task Logout(int accountId);
    }
}
