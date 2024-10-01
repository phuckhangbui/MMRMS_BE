using DTOs.MaintainingTicket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/maintenance-ticket")]
    public class MaintenanceTicketController : BaseApiController
    {
        private readonly IMaintenanceTicketService _maintenanceTicketService;

        public MaintenanceTicketController(IMaintenanceTicketService maintenanceTicketService)
        {
            _maintenanceTicketService = maintenanceTicketService;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult<IEnumerable<MaintaningTicketDto>>> GetMaintenanceRequests()
        {
            try
            {
                IEnumerable<MaintaningTicketDto> list = await _maintenanceTicketService.GetMaintenanceTickets();
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
        public async Task<ActionResult<IEnumerable<MaintaningTicketDto>>> GetMaintenanceRequestsForCustomer()
        {
            int customerId = GetLoginAccountId();
            if (customerId == 0)
            {
                return Unauthorized();
            }

            try
            {
                IEnumerable<MaintaningTicketDto> list = await _maintenanceTicketService.GetMaintenanceTickets(customerId);
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

        [HttpPost]
        [Authorize(Policy = "Staff")]
        public async Task<ActionResult> CreateMaintenanceTicket(CreateMaintaningTicketDto createMaintaningTicketDto)
        {
            int staffId = GetLoginAccountId();
            if (staffId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _maintenanceTicketService.CreateMaintenanceRequest(staffId, createMaintaningTicketDto);
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
