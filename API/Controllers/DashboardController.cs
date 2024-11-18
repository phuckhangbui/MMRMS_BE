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

        [HttpGet("manager")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> GetDataTotalManagerDashboard([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var data = await _dashboardService.GetDataTotalManagerDashboard(startDate, endDate);
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

        [HttpGet("manager/contracts")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> GetDataContractManagerDashboard([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var data = await _dashboardService.GetDataContractManagerDashboard(startDate, endDate);
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

        [HttpGet("manager/money")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> GetDataMoneyManagerDashboard([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var data = await _dashboardService.GetDataMoneyManagerDashboard(startDate, endDate);
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

        [HttpGet("manager/machine-check-request")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> GetDataMachineCheckRequestManagerDashboard([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var data = await _dashboardService.GetDataMachineCheckRequestManagerDashboard(startDate, endDate);
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
