﻿using DTOs.Delivery;
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



        [HttpGet("manager")]
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

        [HttpGet("technical-staff")]
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

        [HttpGet("customer")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<IEnumerable<DeliveryTaskDto>>> GetDeliveriesForCustomer()
        {
            int customerId = GetLoginAccountId();

            try
            {
                IEnumerable<DeliveryTaskDto> list = await _deliverService.GetDeliveriesForCustomer(customerId);
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

        [HttpGet("{deliveryTaskId}/detail")]
        [Authorize]
        public async Task<ActionResult<DeliveryTaskDetailDto>> GetDeliveryDetail([FromRoute] int deliveryTaskId)
        {
            try
            {
                var delivery = await _deliverService.GetDeliveryDetail(deliveryTaskId);
                return Ok(delivery);
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
                return Created();
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

        [HttpPatch("{deliveryTaskId}/staff-change-to-delivering")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult> UpdateStatusToDelivering([FromRoute] int deliveryTaskId)
        {
            int accountId = GetLoginAccountId();

            try
            {
                await _deliverService.UpdateDeliveryStatusToDelivering(deliveryTaskId, accountId);
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

        [HttpPatch("{deliveryTaskId}/manager-change-to-processed-after-failure")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> UpdateStatusToProcessedAfterFailure([FromRoute] int deliveryTaskId)
        {
            int accountId = GetLoginAccountId();

            try
            {
                await _deliverService.UpdateDeliveryStatusToProcessedAfterFailure(deliveryTaskId, accountId);
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

        [HttpPatch("{contractDeliveryId}/manager-change-contract-delivery-to-processed-after-failure")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult> UpdateContractDeliveryStatusToProcessedAfterFailure([FromRoute] int contractDeliveryId)
        {
            int accountId = GetLoginAccountId();

            try
            {
                await _deliverService.UpdateContractDeliveryStatusToProcessedAfterFailure(contractDeliveryId, accountId);
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

        [HttpPut("complete")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult> CompleteDelivery(StaffUpdateDeliveryTaskDto staffUpdateDeliveryTaskDto)
        {
            int accountId = GetLoginAccountId();

            try
            {
                await _deliverService.StaffCompleteDelivery(staffUpdateDeliveryTaskDto, accountId);
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

        [HttpPut("fail-all")]
        [Authorize(Policy = "TechnicalStaff")]
        public async Task<ActionResult> FailDelivery(StaffFailDeliveryTaskDto staffFailDeliveryTask)
        {
            int accountId = GetLoginAccountId();

            try
            {
                await _deliverService.StaffFailDelivery(staffFailDeliveryTask, accountId);
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
