using DTOs.EmployeeTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/tasks")]
    public class EmployeeTaskController : BaseApiController
    {
        private readonly IEmployeeTaskService _employeeTaskService;

        public EmployeeTaskController(IEmployeeTaskService employeeTaskService)
        {
            _employeeTaskService = employeeTaskService;
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetEmployeeTasksForManager()
        {
            try
            {
                IEnumerable<EmployeeTaskDto> list = await _employeeTaskService.GetEmployeeTasks();
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

        [HttpGet("staff")]
        [Authorize(Policy = "Staff")]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetEmployeeTasksForStaff()
        {
            int staffId = GetLoginAccountId();
            if (staffId == 0)
            {
                return Unauthorized();
            }

            try
            {
                IEnumerable<EmployeeTaskDto> list = await _employeeTaskService.GetEmployeeTasks(staffId);
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

        [HttpPatch("{employeeTaskId}")]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult> UpdateEmployeeTaskStatus([FromRoute] int employeeTaskId, [FromQuery] string status)
        {
            try
            {
                await _employeeTaskService.UpdateEmployeeTaskStatus(employeeTaskId, status);
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

        [HttpDelete("{taskId}")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> DeleteEmployeeTask([FromRoute] int taskId)
        {
            try
            {
                await _employeeTaskService.DeleteEmployeeTask(taskId);
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
