using DTOs.HiringRequest;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/hirings")]
    public class HiringController : BaseApiController
    {
        private readonly IHiringService _hiringService;

        public HiringController(IHiringService hiringService)
        {
            _hiringService = hiringService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HiringRequestDto>>> GetHiringRequests()
        {
            try
            {
                var hiringRequests = await _hiringService.GetAll();
                return Ok(hiringRequests);
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
