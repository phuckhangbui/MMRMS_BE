﻿using DTOs.ComponentReplacementTicket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/component-replacement-ticket")]
    public class ComponentReplacementTicketController : BaseApiController
    {
        private readonly IComponentReplacementTicketService _componentReplacementTicketService;

        public ComponentReplacementTicketController(IComponentReplacementTicketService ComponentReplacementTicketService)
        {
            _componentReplacementTicketService = ComponentReplacementTicketService;
        }

        [HttpGet("manager")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<IEnumerable<ComponentReplacementTicketDto>>> GetComponentReplacementTicket()
        {
            try
            {
                IEnumerable<ComponentReplacementTicketDto> list = await _componentReplacementTicketService.GetComponentReplacementTickets();
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
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<IEnumerable<ComponentReplacementTicketDto>>> GetComponentReplacementTicketByContractId([FromRoute] string contractId)
        {
            try
            {
                IEnumerable<ComponentReplacementTicketDto> list = await _componentReplacementTicketService.GetComponentReplacementTicketsByContractId(contractId);
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

        [HttpGet("serial-number/{serialNumber}")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult<IEnumerable<ComponentReplacementTicketDto>>> GetComponentReplacementTicketBySerialNumber([FromRoute] string serialNumber)
        {
            try
            {
                IEnumerable<ComponentReplacementTicketDto> list = await _componentReplacementTicketService.GetComponentReplacementTicketsBySerialNumber(serialNumber);
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

        [HttpGet("technical-staff")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult<IEnumerable<ComponentReplacementTicketDto>>> GetComponentReplacementTicketForStaff()
        {
            int staffId = GetLoginAccountId();

            try
            {
                IEnumerable<ComponentReplacementTicketDto> list = await _componentReplacementTicketService.GetComponentReplacementTicketsForStaff(staffId);
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
        public async Task<ActionResult<IEnumerable<ComponentReplacementTicketDto>>> GetComponentReplacementTicketsForCustomer()
        {
            int customerId = GetLoginAccountId();

            try
            {
                IEnumerable<ComponentReplacementTicketDto> list = await _componentReplacementTicketService.GetComponentReplacementTickets(customerId);
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

        [HttpGet("{replacementTicketId}/detail")]
        [Authorize]
        public async Task<ActionResult<ComponentReplacementTicketDetailDto>> GetComponentReplacementTicketDetail([FromRoute] string replacementTicketId)
        {
            try
            {
                var result = await _componentReplacementTicketService.GetComponentReplacementTicket(replacementTicketId);
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

        [HttpPost("create")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult> CreateComponentReplacementTicket(CreateComponentReplacementTicketDto createComponentReplacementTicketDto)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _componentReplacementTicketService.CreateComponentReplacementTicket(staffId, createComponentReplacementTicketDto);
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

        [HttpPatch("{componentReplacementTicketId}/complete")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult> CompleteComponentReplacementTicket([FromRoute] string componentReplacementTicketId)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _componentReplacementTicketService.CompleteComponentReplacementTicket(staffId, componentReplacementTicketId);
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

        [HttpPatch("{componentReplacementTicketId}/cancel")]
        [Authorize(Policy = "CustomerAndTechnicalStaff")]
        public async Task<ActionResult> CancelComponentReplacementTicket([FromRoute] string componentReplacementTicketId, [FromBody] CancelTicketDto? cancelTicketDto)
        {
            int accountId = GetLoginAccountId();

            try
            {
                string note = null;
                if (cancelTicketDto != null)
                {
                    note = cancelTicketDto.Note;
                }

                await _componentReplacementTicketService.CancelComponentReplacementTicket(accountId, componentReplacementTicketId, note);
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
