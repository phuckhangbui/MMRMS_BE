using DTOs.MachineCheckRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet]
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

        [HttpGet("{MachineCheckRequestId}")]
        public async Task<ActionResult<IEnumerable<MachineCheckRequestDto>>> GetMachineCheckRequest([FromRoute] string MachineCheckRequestId)
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
                await _machineCheckRequestService.CreateMachineCheckRequest(customerId, createMachineCheckRequestDto);
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
        //[HttpPatch("{machineCheckRequestId}")]
        //[Authorize(Policy = "ManagerAndStaff")]
        //public async Task<ActionResult> UpdateMaintenanceStatus([FromRoute] string machineCheckRequestId, [FromQuery] string status)
        //{
        //    int accountId = GetLoginAccountId();
        //    if (accountId == 0)
        //    {
        //        return Unauthorized();
        //    }

        //    try
        //    {
        //        await _machineCheckRequestService.UpdateRequestStatus(machineCheckRequestId, status, accountId);
        //        return NoContent();
        //    }
        //    catch (ServiceException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
