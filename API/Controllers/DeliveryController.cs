using DTOs.Delivery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/deliveries")]
    public class DeliveryController : BaseApiController
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }



        [HttpGet]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<DeliveryDto>>> GetDeliveriesForManager()
        {
            try
            {
                IEnumerable<DeliveryDto> list = await _deliveryService.GetDeliveries();
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
        public async Task<ActionResult<IEnumerable<DeliveryDto>>> GetDeliveriesForStaff()
        {
            int staffId = GetLoginAccountId();
            if (staffId == 0)
            {
                return Unauthorized();
            }

            try
            {
                IEnumerable<DeliveryDto> list = await _deliveryService.GetDeliveries(staffId);
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
        public async Task<ActionResult> AssignDelivery([FromBody] AssignDeliveryDto assignDeliveryDto)
        {
            int managerId = GetLoginAccountId();
            if (managerId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _deliveryService.AssignDelivery(managerId, assignDeliveryDto);
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
        [HttpPatch("{deliveryId}")]
        [Authorize(Policy = "ManagerAndStaff")]
        public async Task<ActionResult> UpdateDeliveryStatus([FromRoute] int deliveryId, [FromQuery] string status)
        {
            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return Unauthorized();
            }


            try
            {
                await _deliveryService.UpdateDeliveryStatus(deliveryId, status, accountId);
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
        //public async Task<IActionResult> CreateDelivery([FromBody] CreateComponentDto createComponentDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
        //        return BadRequest(errorMessages);
        //    }

        //    try
        //    {
        //        await _deliveryService.CreateComponent(createComponentDto);
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
        //        await _deliveryService.UpdateComponent(updateComponentDto);
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
        //        await _deliveryService.UpdateComponentStatus(componentId, status);
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
        //        await _deliveryService.DeleteComponent(componentId);
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
