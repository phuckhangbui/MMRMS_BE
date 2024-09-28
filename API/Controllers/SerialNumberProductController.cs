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
                return StatusCode(201);
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
                return StatusCode(201);
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
