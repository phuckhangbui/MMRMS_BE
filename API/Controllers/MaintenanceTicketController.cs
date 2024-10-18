using DTOs.MaintenanceTicket;
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

        //TODO:KHANG
        [HttpGet]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult<IEnumerable<MaintenanceTicketDto>>> GetMaintenanceRequests()
        {
            try
            {
                IEnumerable<MaintenanceTicketDto> list = await _maintenanceTicketService.GetMaintenanceTickets();
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
        public async Task<ActionResult<IEnumerable<MaintenanceTicketDto>>> GetMaintenanceRequestsForCustomer()
        {
            int customerId = GetLoginAccountId();
            if (customerId == 0)
            {
                return Unauthorized();
            }

            try
            {
                IEnumerable<MaintenanceTicketDto> list = await _maintenanceTicketService.GetMaintenanceTickets(customerId);
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
        public async Task<ActionResult> CreateMaintenanceTicket(CreateMaintenanceTicketDto createMaintenanceTicketDto)
        {
            int staffId = GetLoginAccountId();
            if (staffId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _maintenanceTicketService.CreateMaintenanceTicket(staffId, createMaintenanceTicketDto);
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
