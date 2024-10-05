using DTOs.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetDataTotalAdminDashboard([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var data = await _dashboardService.GetDataTotalAdminDashboard(startDate, endDate);
                return Ok(data);
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

        [HttpGet("/monthly")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<List<DataUserAdminDto>>> GetMonthlyCustomerData([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var data = await _dashboardService.GetMonthlyCustomerDataAsync(startDate, endDate);
                return Ok(data);
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
