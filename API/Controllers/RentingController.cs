using Common;
using DTOs.Contract;
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
        private readonly IContractService _contractService;

        public RentingController(IRentingRequestService rentingService, IContractService contractService)
        {
            _rentingService = rentingService;
            _contractService = contractService;
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

        [HttpGet("{rentingRequestId}/contracts")]
        public async Task<ActionResult<IEnumerable<ContractDetailDto>>> GetContractDetailListByRentingRequestId(string rentingRequestId)
        {
            try
            {
                var contracts = await _contractService.GetContractDetailListByRentingRequestId(rentingRequestId);
                return Ok(contracts);
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

        [HttpPut("{rentingRequestId}/contracts/sign")]
        public async Task<ActionResult> SignContract(string rentingRequestId)
        {
            try
            {
                await _contractService.SignContract(rentingRequestId);
                return Ok(MessageConstant.Contract.SignContractSuccessfully);
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

        [HttpPut("{rentingRequestId}/cancel")]
        public async Task<ActionResult> CancelRentingRequest(string rentingRequestId)
        {
            try
            {
                var result = await _rentingService.CancelRentingRequest(rentingRequestId);
                if (!result) return Ok(MessageConstant.RentingRequest.RentingRequestCancelFail);
                return BadRequest(MessageConstant.RentingRequest.RentingRequestCancelSuccessfully);
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
