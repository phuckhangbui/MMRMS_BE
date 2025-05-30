﻿using DTOs.MachineCheckRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/machine-check-request")]
    public class MachineCheckRequestController : BaseApiController
    {
        private readonly IMachineCheckRequestService _machineCheckRequestService;

        public MachineCheckRequestController(IMachineCheckRequestService MachineCheckRequestService)
        {
            _machineCheckRequestService = MachineCheckRequestService;
        }


        [HttpGet("manager")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<MachineCheckRequestDto>>> GetMachineCheckRequestsForManager()
        {
            try
            {
                IEnumerable<MachineCheckRequestDto> list = await _machineCheckRequestService.GetMachineCheckRequests();
                return Ok(list);
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

        [HttpGet("manager/new")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<MachineCheckRequestDto>>> GetMachineCheckRequestsNewForManager()
        {
            try
            {
                IEnumerable<MachineCheckRequestDto> list = await _machineCheckRequestService.GetMachineCheckRequestsNew();
                return Ok(list);
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

        [HttpGet("customer")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<IEnumerable<MachineCheckRequestDto>>> GetMachineCheckRequestsForCustomer()
        {
            int customerId = GetLoginAccountId();

            try
            {
                IEnumerable<MachineCheckRequestDto> list = await _machineCheckRequestService.GetMachineCheckRequests(customerId);
                return Ok(list);
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

        [HttpGet("contract/{contractId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MachineCheckRequestDto>>> GetMachineCheckRequestsForContract(string contractId)
        {

            try
            {
                IEnumerable<MachineCheckRequestDto> list = await _machineCheckRequestService.GetMachineCheckRequestsOfContract(contractId);
                return Ok(list);
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

        [HttpGet("{machineCheckRequestId}/detail")]
        [Authorize]
        public async Task<ActionResult<MachineCheckRequestDetailDto>> GetMachineCheckRequest([FromRoute] string machineCheckRequestId)
        {
            try
            {
                MachineCheckRequestDetailDto result = await _machineCheckRequestService.GetMachineCheckRequestDetail(machineCheckRequestId);
                return Ok(result);
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

        [HttpGet("criterias")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MachineCheckCriteriaDto>>> GetMachineCheckCriteria()
        {

            try
            {
                IEnumerable<MachineCheckCriteriaDto> list = await _machineCheckRequestService.GetMachineCheckCriterias();
                return Ok(list);
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

        [HttpPost("criterias")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> CreateMachineCheckCriteria([FromForm, BindRequired] string name)
        {
            try
            {
                await _machineCheckRequestService.CreateMachineCheckCriteria(name);
                return Created("", name);
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

        [HttpPut("criterias")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineCheckCriteria(int id, [FromForm, BindRequired] string name)
        {
            try
            {
                var result = await _machineCheckRequestService.UpdateMachineCheckCriteria(id, name);
                if (result) return NoContent();
                return BadRequest();
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

        [HttpDelete("criterias")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> DeleteMachineCheckCriteria(int id)
        {
            try
            {
                var result = await _machineCheckRequestService.DeleteMachineCheckCriteria(id);
                if (result) return NoContent();
                return BadRequest();
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
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult> CreateMachineCheckRequest(CreateMachineCheckRequestDto createMachineCheckRequestDto)
        {
            int customerId = GetLoginAccountId();

            try
            {
                await _machineCheckRequestService.CreateMachineCheckRequest(customerId, createMachineCheckRequestDto);
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

        [HttpPatch("customer/{machineCheckRequestId}/cancel")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult> CancelMachineCheckRequest([FromRoute] string machineCheckRequestId)
        {
            int customerId = GetLoginAccountId();
            try
            {
                await _machineCheckRequestService.CancelMachineCheckRequestDetail(machineCheckRequestId, customerId);
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
