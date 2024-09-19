using DTOs.Account;
using DTOs.Authentication;
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
        public async Task<ActionResult<AccountBaseDto>> LoginUsername(LoginUsernameDto loginDto)
        {

            try
            {
                var user = await _authenticationService.Login(loginDto);

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
    }
}
