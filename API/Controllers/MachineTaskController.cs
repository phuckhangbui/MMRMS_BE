using DTOs;
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

        [HttpGet("manager")]
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

        [HttpGet("technical-staff")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult<IEnumerable<MachineTaskDto>>> GetMachineTasksForStaff()
        {
            int staffId = GetLoginAccountId();

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

        [HttpGet("{taskId}/detail")]
        [Authorize(Policy = "ManagerAndTechnicalStaff")]
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
        public async Task<IActionResult> CreateMachineTaskCheckMachine([FromBody] CreateMachineTaskCheckRequestDto createMachineTaskDto)
        {
            int managerId = GetLoginAccountId();

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

        [HttpPost("check-machine-termination")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateMachineTaskCheckMachineTermination([FromBody] CreateMachineTaskContractTerminationDto createMachineTaskDto)
        {
            int managerId = GetLoginAccountId();

            try
            {
                await _machineTaskService.CreateMachineTaskCheckMachineContractTermination(managerId, createMachineTaskDto);
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

        [HttpPost("check-machine-delivery-fail")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateMachineTaskCheckMachineDeliveryFail([FromBody] CreateMachineTaskDeliveryFailDto createMachineTaskDto)
        {
            int managerId = GetLoginAccountId();

            try
            {
                await _machineTaskService.CreateMachineTaskCheckMachineDeliveryFail(managerId, createMachineTaskDto);
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

        [HttpPatch("{taskId}/check-machine-success")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<IActionResult> CheckMachineSuccess([FromRoute] int taskId, [FromBody] PictureUrlDto confirmationPicture)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _machineTaskService.StaffCheckMachineSuccess(taskId, staffId, confirmationPicture.Url);
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



        //[HttpPatch("{taskId}/replace-component-success")]
        //[Authorize(Policy = "TechnicalStaff")]
        //public async Task<IActionResult> ReplaceComponentSuccess([FromRoute] int taskId, [FromBody] PictureUrlDto confirmationPicture)
        //{
        //    int staffId = GetLoginAccountId();

        //    try
        //    {
        //        await _machineTaskService.StaffReplaceComponentSuccess(taskId, staffId, confirmationPicture.Url);
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

        //[HttpPost("create/next-task")]
        //[Authorize(Policy = "TechnicalStaff")]
        //public async Task<IActionResult> CreateNextTaskForFailTask()
        //{
        //    int staffId = GetLoginAccountId();

        //    try
        //    {

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


        //TODO:KHANG
        //[HttpPost("process-maintenance-ticket")]
        //[Authorize(Policy = "Manager")]
        //public async Task<IActionResult> CreateMachineTaskProcessComponentReplacementTicket([FromBody] CreateMachineTaskProcessComponentReplacementTicket createMachineTaskDto)
        //{
        //    int managerId = GetLoginAccountId();

        //    try
        //    {
        //        await _machineTaskService.CreateMachineTaskProcessComponentReplacementTicket(managerId, createMachineTaskDto);
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

        //[HttpPatch("{MachineTaskId}")]
        //[Authorize(Policy = "ManagerAndTechnicalStaff")]
        //public async Task<ActionResult> UpdateMachineTaskStatus([FromRoute] int MachineTaskId, [FromQuery] string status)
        //{
        //    int accountId = GetLoginAccountId();

        //    try
        //    {
        //        await _machineTaskService.UpdateMachineTaskStatus(MachineTaskId, status, accountId);
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
