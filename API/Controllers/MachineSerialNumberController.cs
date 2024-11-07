using DTOs.MachineSerialNumber;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/machine-serial-numbers")]
    public class MachineSerialNumberController : BaseApiController
    {
        private readonly IMachineSerialNumberService _machineSerialNumberService;

        public MachineSerialNumberController(IMachineSerialNumberService machineSerialNumberService)
        {
            _machineSerialNumberService = machineSerialNumberService;
        }

        [HttpPost]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<IActionResult> CreateMachineSerialNumber([FromBody] MachineSerialNumberCreateRequestDto createSerialMachineNumberDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _machineSerialNumberService.CreateMachineSerialNumber(createSerialMachineNumberDto, accountId);
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

        [HttpGet("{serialNumber}/detail-log")]
        public async Task<ActionResult<IEnumerable<MachineSerialNumberLogDto>>> GetMachineSerialNumberDetailLog([FromRoute] string serialNumber)
        {

            try
            {
                IEnumerable<MachineSerialNumberLogDto> logs = await _machineSerialNumberService.GetDetailLog(serialNumber);
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

        [HttpGet("{serialNumber}/components")]
        public async Task<ActionResult<IEnumerable<MachineSerialNumberComponentDto>>> GetMachineSerialNumberComponentList([FromRoute] string serialNumber)
        {

            try
            {
                IEnumerable<MachineSerialNumberComponentDto> lists = await _machineSerialNumberService.GetSerialNumberComponents(serialNumber);
                return Ok(lists);
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

        [HttpDelete("{serialNumber}")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<IActionResult> DeleteMachineSerialNumber([FromRoute] string serialNumber)
        {

            try
            {
                await _machineSerialNumberService.Delete(serialNumber);
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

        [HttpPatch("{serialNumber}/toggle-lock")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<IActionResult> ToggleMachineSerialNumberStatus([FromRoute] string serialNumber)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _machineSerialNumberService.ToggleStatus(serialNumber, staffId);
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

        [HttpPatch("{serialNumber}/webstaff/machine-active-to-maintenance")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<IActionResult> MachineCheckWhileInStoreMoveToMaintenance([FromRoute] string serialNumber, [FromQuery] string note)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _machineSerialNumberService.MoveSerialMachineToMaintenanceStatus(serialNumber, staffId, note);
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

        [HttpPatch("{serialNumber}/webstaff/machine-maintenance-to-active")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<IActionResult> MachineCheckWhileInStoreMoveToActive([FromRoute] string serialNumber, [FromQuery] string note)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _machineSerialNumberService.MoveSerialMachineToActiveStatus(serialNumber, staffId, note);
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

        [HttpPatch("{machineSerialNumberComponentId}/webstaff/update-component-status/broken")]
        [Authorize("WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineSerialNumberComponentStatusToBrokenWhileInStore([FromRoute] int machineSerialNumberComponentId, [FromQuery] string note)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _machineSerialNumberService.UpdateMachineSerialNumberComponentStatusToBrokenWhileInStore(machineSerialNumberComponentId, staffId, note);
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

        [HttpPatch("{machineSerialNumberComponentId}/webstaff/replace-broken-component")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<IActionResult> ReplaceBrokenComponentWhenNotRenting([FromRoute] int machineSerialNumberComponentId, [FromQuery] bool isDeductFromComponentStorage, [FromQuery] int quantity, [FromQuery] string note)
        {
            int staffId = GetLoginAccountId();

            try
            {
                await _machineSerialNumberService.UpdateMachineSerialNumberComponentStatusToNormalWhileInStore(machineSerialNumberComponentId, staffId, isDeductFromComponentStorage, quantity, note);
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

        //[HttpPut("{serialNumber}")]
        //public async Task<IActionResult> UpdateMachineSerialNumber([FromRoute] string serialNumber, [FromBody] MachineSerialNumberUpdateDto machineSerialNumberUpdateDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
        //        return BadRequest(errorMessages);
        //    }
        //    try
        //    {
        //        await _machineSerialNumberService.Update(serialNumber, machineSerialNumberUpdateDto);
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
