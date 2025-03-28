﻿using Common;
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
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<RentingRequestDto>>> GetRentingRequests([FromQuery] string? status = null)
        {
            try
            {
                var rentingRequests = await _rentingService.GetRentingRequests(status);
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

        [HttpGet("delivery")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<RentingRequestDto>>> GetRentingRequestsStillDelivery()
        {
            try
            {
                var rentingRequests = await _rentingService.GetRentingRequestsThatStillHaveContractNeedDelivery();
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
        public async Task<ActionResult<RentingRequestDetailDto>> GetRentingRequestDetail(string rentingRequestId)
        {
            try
            {
                var rentingRequest = await _rentingService.GetRentingRequestDetail(rentingRequestId);
                return Ok(rentingRequest);
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
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult<string>> CreateRentingRequest([FromBody] NewRentingRequestDto newRentingRequestDto)
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
        public async Task<ActionResult<RentingRequestInitDataDto>> InitializeRentingRequest([FromBody] RentingRequestMachineInRangeDto rentingRequestMachineInRangeDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                int customerId = GetLoginAccountId();
                var rentingRequests = await _rentingService.InitializeRentingRequestData(customerId, rentingRequestMachineInRangeDto);
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
        public async Task<ActionResult<IEnumerable<CustomerRentingRequestDto>>> GetRentingRequestsForCustomer()
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
        [Authorize(policy: "Customer")]
        public async Task<ActionResult> CancelRentingRequest(string rentingRequestId)
        {
            try
            {
                var result = await _rentingService.CancelRentingRequest(rentingRequestId);
                if (result) return Ok(MessageConstant.RentingRequest.RentingRequestCancelSuccessfully);
                return BadRequest(MessageConstant.RentingRequest.RentingRequestCancelFail);
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

        [HttpPost("review-data")]
        [Authorize(policy: "Customer")]
        public async Task<ActionResult<RentingRequestInitDataDto>> ReviewRentingRequest([FromBody] RentingRequestReviewDto rentingRequestReviewDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                int customerId = GetLoginAccountId();
                var rentingRequests = _rentingService.GetRentingRequestReview(rentingRequestReviewDto);
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
