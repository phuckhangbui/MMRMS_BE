﻿using DTOs.DeliveryTask;
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

        [HttpGet("staff")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult<IEnumerable<DeliveryTaskDto>>> GetDeliveriesForStaff()
        {
            int staffId = GetLoginAccountId();

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

        [HttpPost("create")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> CreateDeliveryTask([FromBody] CreateDeliveryTaskDto createDeliveryTaskDto)
        {
            int managerId = GetLoginAccountId();

            try
            {
                await _deliverService.CreateDeliveryTask(managerId, createDeliveryTaskDto);
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
        [HttpPatch("{deliveryTaskId}")]
        [Authorize(Policy = "ManagerAndTechnicalStaff")]
        public async Task<ActionResult> UpdateDeliveryTasktatus([FromRoute] int deliveryTaskId, [FromQuery] string status)
        {
            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return Unauthorized();
            }


            try
            {
                await _deliverService.UpdateDeliveryTaskStatus(deliveryTaskId, status, accountId);
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
