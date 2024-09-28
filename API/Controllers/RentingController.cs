using DTOs.Content;
using DTOs.RentingRequest;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/rentings")]
    public class RentingController : BaseApiController
    {
        private readonly IRentingRequestService _rentingService;

        public RentingController(IRentingRequestService rentingService)
        {
            _rentingService = rentingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentingRequestDto>>> GetRentingRequests()
        {
            try
            {
                var rentingRequests = await _rentingService.GetAll();
                return Ok(rentingRequests);
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

        [HttpPost]
        public async Task<ActionResult> CreateRentingRequest([FromBody] NewRentingRequestDto newRentingRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _rentingService.CreateRentingRequest(newRentingRequestDto);
                return Created("", newRentingRequestDto);
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
