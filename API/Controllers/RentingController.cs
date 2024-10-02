using DTOs.RentingRequest;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{rentingRequestId}")]
        public async Task<ActionResult<RentingRequestDetailDto>> GetRentingRequesteDetailById(string rentingRequestId)
        {
            try
            {
                var rentingRequest = await _rentingService.GetRentingRequestDetailById(rentingRequestId);
                return Ok(rentingRequest);
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
        [Authorize(policy: "Customer")]
        public async Task<ActionResult> CreateRentingRequest([FromBody] NewRentingRequestDto newRentingRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                int customerId = GetLoginAccountId();
                await _rentingService.CreateRentingRequest(customerId, newRentingRequestDto);
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

        [HttpGet("init-data/{productIds}")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult<RentingRequestInitDataDto>> GetRentingRequestInitData([FromRoute] string productIds)
        {
            try
            {
                if (string.IsNullOrEmpty(productIds))
                {
                    return BadRequest();
                }

                int customerId = GetLoginAccountId();
                var productIdList = productIds.Split(',').Select(int.Parse).ToList();
                var rentingRequests = await _rentingService.GetRentingRequestInitData(customerId, productIdList);
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
