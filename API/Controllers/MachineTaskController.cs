﻿using DTOs;
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

        [HttpPost("check-machine-termination")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateMachineTaskCheckMachineTermination([FromBody] CreateMachineTaskContractTerminationDto createMachineTaskDto)
        {
            int managerId = GetLoginAccountId();

            try
            {
                await _machineTaskService.CreateMachineTaskCheckMachineContractTermination(managerId, createMachineTaskDto);
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

        [HttpPost("check-machine-delivery-fail")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> CreateMachineTaskCheckMachineDeliveryFail([FromBody] CreateMachineTaskDeliveryFailDto createMachineTaskDto)
        {
            int managerId = GetLoginAccountId();

            try
            {
                await _machineTaskService.CreateMachineTaskCheckMachineDeliveryFail(managerId, createMachineTaskDto);
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
    }
}
