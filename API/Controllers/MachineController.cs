using DTOs.Machine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/machines")]
    public class MachineController : BaseApiController
    {
        private readonly IMachineService _productService;

        public MachineController(IMachineService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineDto>>> GetMachines()
        {
            try
            {
                var products = await _productService.GetMachineList();
                return Ok(products);
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
                var products = await _productService.GetTop8LatestMachineList();
                return Ok(products);
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

        [HttpGet("review/{productIds}")]
        public async Task<ActionResult<IEnumerable<MachineDto>>> GetMachinesReview([FromRoute] string productIds)
        {
            try
            {
                if (string.IsNullOrEmpty(productIds))
                {
                    return BadRequest();
                }

                var productIdList = productIds.Split(',').Select(int.Parse).ToList();
                var products = await _productService.GetMachineReviews(productIdList);
                return Ok(products);
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

        [HttpGet("{productId}")]
        public async Task<ActionResult<DisplayMachineDetailDto>> GetMachineDetail([FromRoute] int productId)
        {
            try
            {
                var product = await _productService.GetMachineDetailDto(productId);
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

        [HttpGet("{productId}/serial-products")]
        public async Task<ActionResult<DisplayMachineDetailDto>> GetSerialMachineList([FromRoute] int productId)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                var product = await _productService.GetSerialMachineList(productId);
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
                var product = await _productService.CreateMachine(createMachineDto);

                return StatusCode(201, new { productId = product.MachineId });
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

        [HttpDelete("{productId}")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> DeleteMachine([FromRoute] int productId)
        {
            try
            {
                await _productService.DeleteMachine(productId);
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

        [HttpPatch("{productId}/toggle-lock")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineStatus([FromRoute] int productId)
        {

            try
            {
                await _productService.ToggleLockStatus(productId);
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

        [HttpPut("{productId}/attribute/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineAttribute([FromRoute] int productId, [FromBody] IEnumerable<CreateMachineAttributeDto> productAttributeDtos)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _productService.UpdateMachineAttribute(productId, productAttributeDtos);
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

        [HttpPut("{productId}/term/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineTerm([FromRoute] int productId, [FromBody] IEnumerable<CreateMachineTermDto> productTermDtos)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _productService.UpdateMachineTerm(productId, productTermDtos);
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

        [HttpPut("{productId}/component/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachineComponent([FromRoute] int productId, [FromBody] ComponentList componentList)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }
            try
            {
                await _productService.UpdateMachineComponent(productId, componentList);
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

        [HttpPut("{productId}/detail/update")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> UpdateMachine([FromRoute] int productId, [FromBody] UpdateMachineDto updateMachineDto)
        {

            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _productService.UpdateMachineDetail(productId, updateMachineDto);
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

        [HttpPut("{productId}/images")]
        [Authorize(policy: "WebsiteStaff")]
        public async Task<ActionResult> ChangeMachineImages(int productId, [FromBody] List<ImageList> imageList)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _productService.ChangeMachineImages(productId, imageList);
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
