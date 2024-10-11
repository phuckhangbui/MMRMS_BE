using DTOs.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/accounts")]
    public class AccountController : BaseApiController
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

        [HttpGet("customers")]
        public async Task<ActionResult> GetCustomerAccounts()
        {
            try
            {
                var accounts = await _accountService.GetCustomerAccounts();
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

        [HttpGet("employees")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetEmployeeAccounts()
        {
            try
            {
                var accounts = await _accountService.GetEmployeeAccounts();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("employees/staffs")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> GetStaffAccounts()
        {
            try
            {
                var accounts = await _accountService.GetStaffAccounts();
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
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _accountService.CreateCustomerAccount(newCustomerAccountDto);
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

        [HttpPost("employees")]
        [Authorize(policy: "AdminAndManager")]
        public async Task<ActionResult> CreateEmployeeAccount([FromBody] NewEmployeeAccountDto newStaffAndManagerAccountDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                var accountId = await _accountService.CreateEmployeeAccount(newStaffAndManagerAccountDto);
                return Created("", new { account = accountId });
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
        public async Task<IActionResult> ChangeAccountStatus(int accountId, [FromQuery, BindRequired] string status)
        {
            try
            {
                await _accountService.ChangeAccountStatus(accountId, status);
                return NoContent();
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

        [HttpGet("employees/{accountId}")]
        [Authorize(policy: "AdminAndManager")]
        public async Task<ActionResult<EmployeeAccountDto>> GetEmployeeAccountById(int accountId)
        {
            try
            {
                var staffOrManagerAccount = await _accountService.GetEmployeeAccountById(accountId);
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
