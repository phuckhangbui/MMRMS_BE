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
        [Authorize(Policy = "Manager")]
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
        public async Task<ActionResult<IEnumerable<ComponentReplacementTicketDto>>> GetMachineCheckRequestsForCustomer()
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

        [HttpPost("create")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult> CreateComponentReplacementTicket(CreateComponentReplacementTicketDto createComponentReplacementTicketDto)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _componentReplacementTicketService.CreateComponentReplacementTicketWhenCheckMachineRenting(staffId, createComponentReplacementTicketDto);
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
