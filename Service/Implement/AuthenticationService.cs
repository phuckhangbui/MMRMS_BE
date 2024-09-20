using AutoMapper;
using DAO.Enum;
using DTOs;
using DTOs.Account;
using DTOs.Authentication;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Service.Implement
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthenticationService(IAccountRepository accountRepository, ITokenService tokenService, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        private LoginAccountDto AdminLogin(LoginUsernameDto loginUsernameDto)
        {
            var adminUsername = ConfigurationHelper.config.GetSection("AdminAccount:Username").Value;
            var adminPassword = ConfigurationHelper.config.GetSection("AdminAccount:Password").Value;

            if (loginUsernameDto.Username.Equals(adminUsername) && loginUsernameDto.Password.Equals(adminPassword))
            {
                var accountDto = new LoginAccountDto
                {
                    AccountId = 0,
                    Username = "Admin",
                    Name = "System Admin",
                    RoleId = (int)AccountRoleEnum.Admin
                };
                accountDto.Token = _tokenService.CreateToken(accountDto);

                return accountDto;
            }

            return null;
        }
        public Task<LoginAccountDto> ForgetPassword(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginAccountDto> Login(LoginUsernameDto loginUsernameDto)
        {
            var adminAccount = AdminLogin(loginUsernameDto);
            if (adminAccount != null)
            {
                return adminAccount;
            }

            AccountDto accountDto = await _accountRepository.GetStaffAndManagerAccountWithUsername(loginUsernameDto.Username);

            if (accountDto == null)
            {
                throw new ServiceException("No account associate with this username");
            }

            if (accountDto.IsDelete.HasValue && (bool)accountDto.IsDelete)
            {
                throw new ServiceException("The account has been deleted");
            }

            using var hmac = new HMACSHA512(accountDto.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUsernameDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != accountDto.PasswordHash[i])
                {
                    throw new ServiceException("Wrong password");
                }
            }

            LoginAccountDto loginAccountDto = _mapper.Map<LoginAccountDto>(accountDto);

            loginAccountDto.Token = _tokenService.CreateToken(loginAccountDto);

            var refreshToken = _tokenService.GenerateRefreshToken();
            loginAccountDto.RefreshToken = refreshToken;
            loginAccountDto.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);


            if (loginAccountDto.Status.Equals("Inactive"))
            {
                throw new ServiceException("The account has been locked, please contact administrator");
            }

            return loginAccountDto;

        }

        public async Task<LoginAccountDto> Login(LoginEmailDto loginEmailDto)
        {
            AccountDto accountDto = await _accountRepository.GetCustomerAccountWithEmail(loginEmailDto.Email);

            if (accountDto == null)
            {
                throw new ServiceException("No account associate with this email");
            }

            if (accountDto.IsDelete.HasValue && (bool)accountDto.IsDelete)
            {
                throw new ServiceException("The account has been deleted");
            }

            using var hmac = new HMACSHA512(accountDto.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginEmailDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != accountDto.PasswordHash[i])
                {
                    throw new ServiceException("Wrong password");
                }
            }

            LoginAccountDto loginAccountDto = _mapper.Map<LoginAccountDto>(accountDto);

            loginAccountDto.Token = _tokenService.CreateToken(loginAccountDto);

            var refreshToken = _tokenService.GenerateRefreshToken();
            loginAccountDto.RefreshToken = refreshToken;
            loginAccountDto.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);


            if (loginAccountDto.Status.Equals("Inactive"))
            {
                throw new ServiceException("The account has been locked");
            }

            return loginAccountDto;
        }

        public async Task Logout(int accountId)
        {
            var account = await _accountRepository.GetAccountDtoById(accountId);

            account.TokenRefresh = null;
            account.TokenDateExpire = DateTime.MinValue;

            await _accountRepository.UpdateAccount(account);
        }

        public async Task<LoginAccountDto> RefreshToken(TokenApiDto tokenApiDto)
        {
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var accountId = int.Parse(principal.Claims.First(i => i.Type == "AccountId").Value);

            var accountDto = await _accountRepository.GetAccountDtoById(accountId);

            if (accountDto is null || accountDto.TokenRefresh != refreshToken || accountDto.TokenDateExpire <= DateTime.Now)
                return null;

            var newAccessToken = _tokenService.CreateToken(principal.Claims);
            //var newRefreshToken = _tokenService.GenerateRefreshToken();

            //accountDto.RefreshToken = newRefreshToken;
            await _accountRepository.UpdateAccount(accountDto);

            var loginAccountDto = _mapper.Map<LoginAccountDto>(accountDto);

            loginAccountDto.Token = newAccessToken;
            return loginAccountDto;
        }
    }
}
