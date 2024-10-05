using DTOs.RentingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/renting-services")]
    public class RentingServiceController : BaseApiController
    {
        private readonly IRentingServiceService _rentingServiceService;

        public RentingServiceController(IRentingServiceService rentingServiceService)
        {
            _rentingServiceService = rentingServiceService;
        }

        [HttpGet]
        public async Task<ActionResult> GetRentingServices()
        {
            try
            {
                var rentingServices = await _rentingServiceService.GetRentingServices();
                return Ok(rentingServices);
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

        [HttpGet("{rentingServiceId}")]
        public async Task<ActionResult<RentingServiceDto>> GetRentingServiceById(int rentingServiceId)
        {
            try
            {
                var rentingService = await _rentingServiceService.GetRentingServiceById(rentingServiceId);
                return Ok(rentingService);
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

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> CreateRentingService([FromBody] RentingServiceRequestDto rentingServiceRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _rentingServiceService.CreateRentingService(rentingServiceRequestDto);
                return Created("", rentingServiceRequestDto);
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

        [HttpPut("{rentingServiceId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> UpdateRentingService(int rentingServiceId, [FromBody] RentingServiceRequestDto rentingServiceRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _rentingServiceService.UpdateRentingService(rentingServiceId, rentingServiceRequestDto);
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

        [HttpDelete("{rentingServiceId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> DeleteRentingService(int rentingServiceId)
        {
            try
            {
                await _rentingServiceService.DeleteRentingService(rentingServiceId);
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
    }
}
