using Common;
using DTOs.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/addresses")]
    public class AddressController : BaseApiController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("customer")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult<List<AddressDto>>> GetAddresses()
        {
            try
            {
                int customerId = GetLoginAccountId();
                var contents = await _addressService.GetAddressesForCustomer(customerId);
                return Ok(contents);
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

        [HttpPost("customer")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult> CreateAddressForCustomer([FromBody] AddressRequestDto addressRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                int customerId = GetLoginAccountId();
                await _addressService.CreateAddressForCustomer(customerId, addressRequestDto);
                return Created("", addressRequestDto);
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

        [HttpPut("{addressId}")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult> UpdateAddress(int addressId, [FromBody] AddressRequestDto addressRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                int accountId = GetLoginAccountId();

                var result = await _addressService.UpdateAddress(accountId, addressId, addressRequestDto);
                if (result) return Ok(MessageConstant.Address.ChangeAddressSuccessfully);
                return BadRequest(MessageConstant.Address.ChangeAddressFail);
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

        [HttpDelete("{addressId}")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult> DeleteAddress(int addressId)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                int accountId = GetLoginAccountId();

                var result = await _addressService.DeleteAddress(accountId, addressId);
                if (result) return Ok(MessageConstant.Address.DeleteAddressSuccessfully);
                return BadRequest(MessageConstant.Address.DeleteAddressFail);
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
    }
}
