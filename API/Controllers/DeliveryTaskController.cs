using DTOs.DeliveryTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/deliveries")]
    public class DeliveryTaskController : BaseApiController
    {
        private readonly IDeliverService _deliverService;

        public DeliveryTaskController(IDeliverService DeliveryTaskervice)
        {
            _deliverService = DeliveryTaskervice;
        }



        [HttpGet]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<DeliveryTaskDto>>> GetDeliveriesForManager()
        {
            try
            {
                IEnumerable<DeliveryTaskDto> list = await _deliverService.GetDeliveries();
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
        public async Task<ActionResult<IEnumerable<DeliveryTaskDto>>> GetDeliveriesForStaff()
        {
            int staffId = GetLoginAccountId();
            if (staffId == 0)
            {
                return Unauthorized();
            }

            try
            {
                IEnumerable<DeliveryTaskDto> list = await _deliverService.GetDeliveries(staffId);
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

        [HttpPost("assign")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> AssignDeliveryTask([FromBody] AssignDeliveryTaskDto assignDeliveryTaskDto)
        {
            int managerId = GetLoginAccountId();
            if (managerId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _deliverService.AssignDeliveryTask(managerId, assignDeliveryTaskDto);
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
        [HttpPatch("{DeliveryTaskId}")]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult> UpdateDeliveryTasktatus([FromRoute] int DeliveryTaskId, [FromQuery] string status)
        {
            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return Unauthorized();
            }


            try
            {
                await _deliverService.UpdateDeliveryTaskStatus(DeliveryTaskId, status, accountId);
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


        //[HttpPost]
        //[Authorize(Policy = "Manager")]
        //public async Task<IActionResult> CreateDeliveryTask([FromBody] CreateComponentDto createComponentDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
        //        return BadRequest(errorMessages);
        //    }

        //    try
        //    {
        //        await _deliverService.CreateComponent(createComponentDto);
        //        return Created();
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

        //[HttpPut]
        //public async Task<IActionResult> UpdateComponent([FromBody] UpdateComponentDto updateComponentDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
        //        return BadRequest(errorMessages);
        //    }

        //    try
        //    {
        //        await _DeliveryTaskervice.UpdateComponent(updateComponentDto);
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

        //[HttpPatch("{componentId}/status")]
        //public async Task<IActionResult> UpdateComponentStatus([FromRoute] int componentId, [FromQuery] string status)
        //{

        //    try
        //    {
        //        await _DeliveryTaskervice.UpdateComponentStatus(componentId, status);
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

        //[HttpDelete("{componentId}")]
        //public async Task<IActionResult> DeleteComponent([FromRoute] int componentId)
        //{
        //    try
        //    {
        //        await _DeliveryTaskervice.DeleteComponent(componentId);
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
