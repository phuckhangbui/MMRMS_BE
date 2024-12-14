using DTOs.Machine;
using DTOs.MachineSerialNumber;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/machines")]
    public class MachineController : BaseApiController
    {
        private readonly IMachineService _machineService;

        public MachineController(IMachineService productService)
        {
            _machineService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineViewDto>>> GetMachines()
        {
            try
            {
                var machines = await _machineService.GetMachineList();
                return Ok(machines);
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

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<MachineDto>>> GetActiveMachines()
        {
            try
            {
                var machines = await _machineService.GetActiveMachines();
                return Ok(machines);
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

        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<MachineDto>>> GetTop8LatestMachines()
        {
            try
            {
                var machines = await _machineService.GetTop8LatestMachineList();
                return Ok(machines);
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

        [HttpGet("review/{machineIds}")]
        public async Task<ActionResult<IEnumerable<MachineReviewDto>>> GetMachinesReview([FromRoute] string machineIds)
        {
            try
            {
                if (string.IsNullOrEmpty(machineIds))
                {
                    return BadRequest();
                }

                var machineIdList = machineIds.Split(',').Select(int.Parse).ToList();
                var machines = await _machineService.GetMachineReviews(machineIdList);
                return Ok(machines);
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

        [HttpGet("{machineId}")]
        public async Task<ActionResult<MachineDetailDto>> GetMachineDetail([FromRoute] int machineId)
        {
            try
            {
                var product = await _machineService.GetMachineDetail(machineId);
                return Ok(product);
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

        [HttpGet("{machineId}/serial-machines")]
        public async Task<ActionResult<MachineSerialNumberDto>> GetSerialMachineList([FromRoute] int machineId)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                var list = await _machineService.GetSerialMachineList(machineId);
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

        [HttpPost]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<IActionResult> CreateMachine([FromBody] CreateMachineDto createMachineDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                var machine = await _machineService.CreateMachine(createMachineDto);

                return StatusCode(201, new { machineId = machine.MachineId });
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

        [HttpDelete("{machineId}")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> DeleteMachine([FromRoute] int machineId)
        {
            try
            {
                await _machineService.DeleteMachine(machineId);
                return Ok();
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

        [HttpPatch("{machineId}/toggle-lock")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineStatus([FromRoute] int machineId)
        {

            try
            {
                await _machineService.ToggleLockStatus(machineId);
                return Ok();
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

        [HttpPut("{machineId}/attribute/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineAttribute([FromRoute] int machineId, [FromBody] IEnumerable<CreateMachineAttributeDto> productAttributeDtos)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _machineService.UpdateMachineAttribute(machineId, productAttributeDtos);
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

        [HttpPut("{machineId}/term/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineTerm([FromRoute] int machineId, [FromBody] IEnumerable<CreateMachineTermDto> productTermDtos)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _machineService.UpdateMachineTerm(machineId, productTermDtos);
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

        [HttpPut("{machineId}/component/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineComponent([FromRoute] int machineId, [FromBody] ComponentList componentList)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _machineService.UpdateMachineComponent(machineId, componentList);
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

        [HttpPut("{machineId}/detail/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachine([FromRoute] int machineId, [FromBody] UpdateMachineDto updateMachineDto)
        {
            var accountId = GetLoginAccountId();

            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _machineService.UpdateMachineDetail(machineId, updateMachineDto, accountId);
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

        [HttpPut("{machineId}/images")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> ChangeMachineImages(int machineId, [FromBody] List<ImageList> imageList)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _machineService.ChangeMachineImages(machineId, imageList);
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

        [HttpGet("{machineId}/quotation")]
        public async Task<ActionResult<MachineQuotationDto>> GetMachineQuotation([FromRoute] int machineId)
        {

            try
            {
                var quotation = await _machineService.GetMachineQuotation(machineId);
                return Ok(quotation);
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

        [HttpGet("quotations")]
        public async Task<ActionResult<List<MachineQuotationDto>>> GetMachineQuotations()
        {

            try
            {
                var quotations = await _machineService.GetMachineQuotations();
                return Ok(quotations);
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
