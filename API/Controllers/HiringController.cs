using DTOs.RentingRequest;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/rentings")]
    public class RentingController : BaseApiController
    {
        private readonly IRentingService _rentingService;

        public RentingController(IRentingService rentingService)
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
    }
}
