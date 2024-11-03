using DTOs.Account;
using DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : BaseApiController
    {

        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("username/login")]
        public async Task<ActionResult<LoginAccountDto>> LoginUsername(LoginUsernameDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                var user = await _authenticationService.LoginUsername(loginDto);

                if (user == null)
                {
                    return Unauthorized("Wrong password");
                }
                return Ok(user);
            }
            catch (ServiceException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("email/login")]
        public async Task<ActionResult<LoginAccountDto>> LoginEmail(LoginEmailDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                var user = await _authenticationService.LoginEmail(loginDto);

                if (user == null)
                {
                    return Unauthorized("Wrong password");
                }
                return Ok(user);
            }
            catch (ServiceException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(NewCustomerAccountDto newCustomerAccountDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _authenticationService.RegisterCustomer(newCustomerAccountDto);

                return Created();
            }
            catch (ServiceException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("activate-account")]
        public async Task<ActionResult> ActivateMemberAccount(MemberConfirmEmailOtpDto memberConfirmEmailOtpDto)
        {
            try
            {
                await _authenticationService.ActivateMemberEmailByOtp(memberConfirmEmailOtpDto);

                return NoContent();
            }
            catch (ServiceException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("send-otp")]
        public async Task<ActionResult> SendOtp([FromQuery] string email)
        {
            try
            {
                await _authenticationService.SendOtp(email);

                return NoContent();
            }
            catch (ServiceException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("change-password/otp")]
        public async Task<ActionResult> ChangePasswordWithOtp(ChangePasswordWithOtpDto memberConfirmOtpWhenForgetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _authenticationService.ConfirmOtpAndChangePasswordWhenForget(memberConfirmOtpWhenForgetPasswordDto);

                return NoContent();
            }
            catch (ServiceException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePasswordWithOldPassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _authenticationService.ChangePasswordWithOldPassword(accountId, changePasswordDto);
                return NoContent();
            }
            catch (ServiceException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("refresh")]
        public async Task<ActionResult<LoginAccountDto>> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid client request");

            try
            {
                var accountDto = await _authenticationService.RefreshToken(tokenApiDto);
                if (accountDto == null)
                {
                    return BadRequest("Invalid token");
                }
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                int accountId = GetLoginAccountId();
                await _authenticationService.Logout(accountId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
