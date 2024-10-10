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
                var rentingRequestId = await _rentingService.CreateRentingRequest(customerId, newRentingRequestDto);
                return Created("", new { rentingRequest = rentingRequestId });
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

        [HttpPost("init-data")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult<RentingRequestInitDataDto>> InitializeRentingRequest([FromBody] RentingRequestProductInRangeDto rentingRequestProductInRangeDto)
        {
            try
            {
                int customerId = GetLoginAccountId();
                var rentingRequests = await _rentingService.InitializeRentingRequestData(customerId, rentingRequestProductInRangeDto);
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

        [HttpGet("customer/requests")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult<IEnumerable<RentingRequestDto>>> GetRentingRequestsForCustomer()
        {
            try
            {
                int customerId = GetLoginAccountId();
                var rentingRequests = await _rentingService.GetRentingRequestsForCustomer(customerId);
                return Ok(rentingRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
