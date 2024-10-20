using DTOs.MachineTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/machine-tasks")]
    public class MachineTaskController : BaseApiController
    {
        private readonly IMachineTaskService _machineTaskService;

        public MachineTaskController(IMachineTaskService MachineTaskService)
        {
            _machineTaskService = MachineTaskService;
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<MachineTaskDto>>> GetMachineTasksForManager()
        {
            try
            {
                IEnumerable<MachineTaskDto> list = await _machineTaskService.GetMachineTasks();
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
        [HttpGet("staff")]
        [Authorize(Policy = "Staff")]
        public async Task<ActionResult<IEnumerable<MachineTaskDto>>> GetMachineTasksForStaff()
        {
            int staffId = GetLoginAccountId();
            if (staffId == 0)
            {
                return Unauthorized();
            }

            try
            {
                IEnumerable<MachineTaskDto> list = await _machineTaskService.GetMachineTasks(staffId);
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
        [HttpGet("{taskId}")]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult<MachineTaskDisplayDetail>> GetMachineTaskDetail([FromRoute] int taskId)
        {
            try
            {
                var taskDetail = await _machineTaskService.GetMachineTaskDetail(taskId);
                return Ok(taskDetail);
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

        [HttpPost("check-machine")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateMachineTaskCheckMachine([FromBody] CreateMachineTaskCheckMachineDto createMachineTaskDto)
        {
            int managerId = GetLoginAccountId();
            if (managerId == 0)
            {
                return Unauthorized();
            }
            try
            {
                await _machineTaskService.CreateMachineTaskCheckMachine(managerId, createMachineTaskDto);
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

        [HttpPost("process-maintenance-ticket")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateMachineTaskProcessComponentReplacementTicket([FromBody] CreateMachineTaskProcessComponentReplacementTickett createMachineTaskDto)
        {
            int managerId = GetLoginAccountId();
            if (managerId == 0)
            {
                return Unauthorized();
            }
            try
            {
                await _machineTaskService.CreateMachineTaskProcessComponentReplacementTicket(managerId, createMachineTaskDto);
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
        [HttpPatch("{MachineTaskId}")]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult> UpdateMachineTaskStatus([FromRoute] int MachineTaskId, [FromQuery] string status)
        {
            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return Unauthorized();
            }
            try
            {
                await _machineTaskService.UpdateMachineTaskStatus(MachineTaskId, status, accountId);
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

        //[HttpDelete("{taskId}")]
        //[Authorize(Policy = "Manager")]
        //public async Task<IActionResult> DeleteMachineTask([FromRoute] int taskId)
        //{
        //    try
        //    {
        //        await _MachineTaskService.DeleteMachineTask(taskId);
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
