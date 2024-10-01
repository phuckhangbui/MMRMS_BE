using DTOs.SerialNumberProduct;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/serial-number-product")]
    public class SerialNumberProductController : BaseApiController
    {
        private readonly ISerialNumberProductService _serialNumberProductService;

        public SerialNumberProductController(ISerialNumberProductService serialNumberProductService)
        {
            _serialNumberProductService = serialNumberProductService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSerialNumberProduct([FromBody] SerialNumberProductCreateRequestDto createSerialProductNumberDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _serialNumberProductService.CreateSerialNumberProduct(createSerialProductNumberDto);
                return Created();
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

        [HttpDelete("{serialNumber}")]
        public async Task<IActionResult> DeleteSerialNumberProduct([FromRoute] string serialNumber)
        {

            try
            {
                await _serialNumberProductService.Delete(serialNumber);
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

        [HttpPatch("{serialNumber}/status")]
        public async Task<IActionResult> UpdateSerialNumberProductStatus([FromRoute] string serialNumber, [FromQuery] string status)
        {

            try
            {
                await _serialNumberProductService.UpdateStatus(serialNumber, status);
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

        [HttpPut("{serialNumber}")]
        public async Task<IActionResult> UpdateSerialNumberProduct([FromRoute] string serialNumber, [FromBody] SerialNumberProductUpdateDto serialNumberProductUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _serialNumberProductService.Update(serialNumber, serialNumberProductUpdateDto);
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

        [HttpGet("renting-requests/{rentingRequestId}")]
        public async Task<IActionResult> GetSerialProductNumbersAvailableForRenting(string rentingRequestId)
        {
            try
            {
                var serialNumberProducts = await _serialNumberProductService.GetSerialProductNumbersAvailableForRenting(rentingRequestId);
                return Ok(serialNumberProducts);
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
