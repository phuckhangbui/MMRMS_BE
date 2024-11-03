using DTOs.LogDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/logs")]
    [Authorize(Policy = "Admin")]
    public class LogController : BaseApiController
    {
        private readonly ILogSerevice _logSerevice;

        public LogController(ILogSerevice logSerevice)
        {
            _logSerevice = logSerevice;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogDetailDto>>> GetLogs()
        {
            try
            {
                var logs = await _logSerevice.GetLogs();
                return Ok(logs);
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
