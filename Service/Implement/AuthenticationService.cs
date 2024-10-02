using AutoMapper;
using Common;
using DAO.Enum;
using DTOs;
using DTOs.Account;
using DTOs.Authentication;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Service.Implement
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public AuthenticationService(IAccountRepository accountRepository, ITokenService tokenService, IMapper mapper, IMailService mailService)
        {
            _accountRepository = accountRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _mailService = mailService;
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

        private async Task<LoginAccountDto> Login(AccountDto accountDto, string password, string? firebaseMessagingToken)
        {
            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            if (accountDto.IsDelete.HasValue && (bool)accountDto.IsDelete)
            {
                throw new ServiceException(MessageConstant.Account.AccountDeleted);
            }

            using var hmac = new HMACSHA512(accountDto.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != accountDto.PasswordHash[i])
                {
                    throw new ServiceException(MessageConstant.Account.WrongPassword);
                }
            }

            if (accountDto.Status?.Equals(AccountStatusEnum.Inactive.ToString()) ?? false)
            {
                throw new ServiceException(MessageConstant.Account.AccountInactive);
            }

            if (accountDto.Status?.Equals(AccountStatusEnum.Locked.ToString()) ?? false)
            {
                throw new ServiceException(MessageConstant.Account.AccountLocked);
            }

            LoginAccountDto loginAccountDto = _mapper.Map<LoginAccountDto>(accountDto);

            loginAccountDto.Token = _tokenService.CreateToken(loginAccountDto);

            var refreshToken = _tokenService.GenerateRefreshToken();
            loginAccountDto.RefreshToken = refreshToken;
            loginAccountDto.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            if (string.IsNullOrEmpty(firebaseMessagingToken))
            {

            }
            else if (!firebaseMessagingToken.Equals(accountDto.FirebaseMessageToken))
            {
                var firebaseTokenExistedAccount = await _accountRepository.FirebaseTokenExisted(firebaseMessagingToken);
                if (firebaseTokenExistedAccount != null)
                {
                    firebaseTokenExistedAccount.FirebaseMessageToken = null;
                    await _accountRepository.UpdateAccount(firebaseTokenExistedAccount);
                }
                accountDto.FirebaseMessageToken = firebaseMessagingToken;
            }
            await _accountRepository.UpdateAccount(accountDto);

            return loginAccountDto;
        }

        public async Task<LoginAccountDto> LoginUsername(LoginUsernameDto loginUsernameDto)
        {
            var adminAccount = AdminLogin(loginUsernameDto);
            if (adminAccount != null)
            {
                return adminAccount;
            }

            AccountDto accountDto = await _accountRepository.GetStaffAndManagerAccountWithUsername(loginUsernameDto.Username);

            return await Login(accountDto, loginUsernameDto.Password, loginUsernameDto.FirebaseMessageToken);
        }

        public async Task<LoginAccountDto> LoginEmail(LoginEmailDto loginEmailDto)
        {
            AccountDto accountDto = await _accountRepository.GetCustomerAccountWithEmail(loginEmailDto.Email);

            return await Login(accountDto, loginEmailDto.Password, loginEmailDto.FirebaseMessageToken);
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

        public async Task RegisterCustomer(NewCustomerAccountDto newCustomerAccountDto)
        {
            AccountDto accountDto = await _accountRepository.GetCustomerAccountWithEmail(newCustomerAccountDto.Email);
            if (accountDto != null)
            {
                throw new ServiceException(MessageConstant.Account.EmailAlreadyExists);
            }

            accountDto = await _accountRepository.CreateCustomerAccount(newCustomerAccountDto);

            //send email opt here
            _mailService.SendMail(AuthenticationMail.SendWelcomeAndOtpToCustomer(accountDto.Email, accountDto.Name, accountDto.OtpNumber));

        }

        public async Task ActivateMemberEmailByOtp(MemberConfirmEmailOtpDto memberConfirmEmailOtpDto)
        {
            AccountDto accountDto = await _accountRepository.GetCustomerAccountWithEmail(memberConfirmEmailOtpDto.Email);
            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            if (!memberConfirmEmailOtpDto.Otp.Equals(accountDto.OtpNumber))
            {
                throw new ServiceException(MessageConstant.Account.WrongOtp);
            }

            accountDto.Status = AccountStatusEnum.Active.ToString();

            await _accountRepository.UpdateAccount(accountDto);
        }

        public async Task SendOtp(string email)
        {
            AccountDto accountDto = await _accountRepository.GetCustomerAccountWithEmail(email);
            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            Random random = new Random();
            var otp = random.Next(111111, 999999).ToString();

            accountDto.OtpNumber = otp;
            await _accountRepository.UpdateAccount(accountDto);

            //send mail here
            _mailService.SendMail(AuthenticationMail.SendOtpToCustomer(accountDto.Email, accountDto.Name, accountDto.OtpNumber));
        }



        public async Task ConfirmOtpAndChangePasswordWhenForget(ChangePasswordWithOtpDto changePasswordWithOtpDto)
        {
            AccountDto accountDto = await _accountRepository.GetCustomerAccountWithEmail(changePasswordWithOtpDto.Email);
            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }

            if (!changePasswordWithOtpDto.Otp.Equals(accountDto.OtpNumber))
            {
                throw new ServiceException(MessageConstant.Account.WrongOtp);
            }

            await _accountRepository.ChangeAccountPassword(accountDto, changePasswordWithOtpDto.Password);
        }

        public async Task ChangePasswordWithOldPassword(int accountId, ChangePasswordDto changePasswordDto)
        {
            AccountDto accountDto = await _accountRepository.GetAccounById(accountId);
            if (accountDto == null)
            {
                throw new ServiceException(MessageConstant.Account.AccountNotFound);
            }


            using var hmac = new HMACSHA512(accountDto.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(changePasswordDto.OldPassword));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != accountDto.PasswordHash[i])
                {
                    throw new ServiceException(MessageConstant.Account.WrongPassword);
                }
            }

            await _accountRepository.ChangeAccountPassword(accountDto, changePasswordDto.Password);
        }
    }
}
