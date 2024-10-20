using DTOs.MachineCheckRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/maintenance-request")]
    public class MachineCheckRequestController : BaseApiController
    {
        private readonly IMachineCheckRequestService _MachineCheckRequestService;

        public MachineCheckRequestController(IMachineCheckRequestService MachineCheckRequestService)
        {
            _MachineCheckRequestService = MachineCheckRequestService;
        }


        [HttpGet]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<MachineCheckRequestDto>>> GetMachineCheckRequestsForManager()
        {
            try
            {
                IEnumerable<MachineCheckRequestDto> list = await _MachineCheckRequestService.GetMachineCheckRequests();
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

        [HttpGet("{MachineCheckRequestId}")]
        public async Task<ActionResult<IEnumerable<MachineCheckRequestDto>>> GetMachineCheckRequest([FromRoute] string MachineCheckRequestId)
        {
            try
            {
                IEnumerable<MachineCheckRequestDto> list = await _MachineCheckRequestService.GetMachineCheckRequests();
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
            if (customerId == 0)
            {
                return Unauthorized();
            }

            try
            {
                IEnumerable<MachineCheckRequestDto> list = await _MachineCheckRequestService.GetMachineCheckRequests(customerId);
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
                IEnumerable<MachineCheckRequestDto> list = await _MachineCheckRequestService.GetMachineCheckRequestsOfContract(contractId);
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
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult> CreateMachineCheckRequest(CreateMachineCheckRequestDto createMachineCheckRequestDto)
        {
            int customerId = GetLoginAccountId();
            if (customerId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _MachineCheckRequestService.CreateMachineCheckRequest(customerId, createMachineCheckRequestDto);
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

        //TODO:KHANG
        [HttpPatch("{MachineCheckRequestId}")]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult> UpdateMaintenanceStatus([FromRoute] string MachineCheckRequestId, [FromQuery] string status)
        {
            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _MachineCheckRequestService.UpdateRequestStatus(MachineCheckRequestId, status, accountId);
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
