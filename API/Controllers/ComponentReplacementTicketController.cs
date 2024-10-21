using DTOs.ComponentReplacementTicket;
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

        //TODO:KHANG
        [HttpGet]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult<IEnumerable<ComponentReplacementTicketDto>>> GetMachineCheckRequests()
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

        [HttpGet("customer")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<IEnumerable<ComponentReplacementTicketDto>>> GetMachineCheckRequestsForCustomer()
        {
            int customerId = GetLoginAccountId();
            if (customerId == 0)
            {
                return Unauthorized();
            }

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

        //TODO:KHANG
        [HttpPost]
        [Authorize(Policy = "Staff")]
        public async Task<ActionResult> CreateComponentReplacementTicket(CreateComponentReplacementTicketDto createComponentReplacementTicketDto)
        {
            int staffId = GetLoginAccountId();
            if (staffId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _componentReplacementTicketService.CreateComponentReplacementTicket(staffId, createComponentReplacementTicketDto);
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
