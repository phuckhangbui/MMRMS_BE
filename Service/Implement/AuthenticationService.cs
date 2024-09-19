
using DTOs;
using DTOs.Account;
using DTOs.Authentication;
using Service.Interface;

namespace Service.Implement
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<AccountBaseDto> ForgetPassword(string email)
        {
            throw new NotImplementedException();
        }

        public Task<AccountBaseDto> Login(LoginUsernameDto loginUsernameDto)
        {
            throw new NotImplementedException();
        }

        public Task<AccountBaseDto> Login(LoginEmailDto loginEmail)
        {
            throw new NotImplementedException();
        }

        public Task Logout(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountBaseDto> RefreshToken(TokenApiDto tokenApiDto)
        {
            throw new NotImplementedException();
        }
    }
}
