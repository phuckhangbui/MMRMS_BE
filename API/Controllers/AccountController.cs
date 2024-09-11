using DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAccounts([FromQuery] int? role)
        {
            try
            {
                var accounts = await _accountService.GetAccountsByRole(role);
                return Ok(accounts);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("managers-staff")]
        public async Task<ActionResult> GetManagerAndStaffAccounts()
        {
            try
            {
                var accounts = await _accountService.GetManagerAndStaffAccountsByRole();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("customers")]
        public async Task<ActionResult> CreateCustomerAccount([FromBody] NewCustomerAccountDto newCustomerAccountDto)
        {
            try
            {
                await _accountService.CreateAccount(newCustomerAccountDto, null);
                return Created("", newCustomerAccountDto);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("staff-manager")]
        public async Task<ActionResult> CreateStaffOrManagerAccount([FromBody] NewStaffAndManagerAccountDto newStaffAndManagerAccountDto)
        {
            try
            {
                await _accountService.CreateAccount(null, newStaffAndManagerAccountDto);
                return Created("", newStaffAndManagerAccountDto);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{accountId}/status")]
        public async Task<IActionResult> ChangeAccountStatus(int accountId, [FromQuery] int status)
        {
            try
            {
                await _accountService.ChangeAccountStatus(accountId, status);
                return NoContent();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("customers/{accountId}")]
        public async Task<ActionResult<CustomerAccountDto>> GetCustomerAccountById(int accountId)
        {
            try
            {
                var customerAccount = await _accountService.GetCustomerAccountById(accountId);
                return Ok(customerAccount);
            }
            catch (ServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("staff-manager/{accountId}")]
        public async Task<ActionResult<StaffAndManagerAccountDto>> GetStaffOrManagerAccountById(int accountId)
        {
            try
            {
                var staffOrManagerAccount = await _accountService.GetStaffAndManagerAccountById(accountId);
                return Ok(staffOrManagerAccount);
            }
            catch (ServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
