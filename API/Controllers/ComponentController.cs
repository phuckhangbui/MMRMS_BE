using DTOs.Component;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/components")]
    public class ComponentController : BaseApiController
    {
        private readonly IComponentService _componentService;

        public ComponentController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentDto>>> GetComponents()
        {
            try
            {
                var components = await _componentService.GetComponents();
                return Ok(components);
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
        public async Task<IActionResult> CreateComponent([FromBody] CreateComponentDto createComponentDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _componentService.CreateComponent(createComponentDto);
                return StatusCode(201);
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

        [HttpPut]
        public async Task<IActionResult> UpdateComponent([FromBody] UpdateComponentDto updateComponentDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _componentService.UpdateComponent(updateComponentDto);
                return StatusCode(201);
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

        [HttpPatch("{componentId}/status")]
        public async Task<IActionResult> UpdateComponentStatus([FromRoute] int componentId, [FromQuery] string status)
        {

            try
            {
                await _componentService.UpdateComponentStatus(componentId, status);
                return StatusCode(201);
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

        [HttpDelete("{componentId}")]
        public async Task<IActionResult> DeleteComponent([FromRoute] int componentId)
        {
            try
            {
                await _componentService.DeleteComponent(componentId);
                return StatusCode(201);
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
