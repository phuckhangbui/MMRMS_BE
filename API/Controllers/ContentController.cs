using DTOs.Content;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace API.Controllers
{
    [Route("api/contents")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet]
        public async Task<ActionResult> GetContents()
        {
            try
            {
                var contents = await _contentService.GetContents();
                return Ok(contents);
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

        [HttpGet("{contentId}")]
        public async Task<ActionResult<ContentDto>> GetContentDetailById(int contentId)
        {
            try
            {
                var content = await _contentService.GetContentDetailById(contentId);
                return Ok(content);
            }
            catch (ServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateContent([FromForm] ContentCreateRequestDto contentRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _contentService.CreateContent(contentRequestDto);
                return Created("", contentRequestDto);
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

        [HttpPut("{contentId}")]
        public async Task<ActionResult> UpdateContent(int contentId, [FromForm] ContentUpdateRequestDto contentUpdateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _contentService.UpdateContent(contentId, contentUpdateRequestDto);
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

        [HttpDelete("{contentId}")]
        public async Task<ActionResult> DeleteContent(int contentId)
        {
            try
            {
                await _contentService.DeleteContent(contentId);
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

        [HttpPatch("{contentId}/status")]
        public async Task<IActionResult> ChangeContentStatus(int contentId, [FromQuery] string status)
        {
            try
            {
                await _contentService.ChangeContentStatus(contentId, status);
                return NoContent();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{contentId}/images")]
        public async Task<ActionResult> ChangeContentImage(int contentId, IFormFile imageUrl)
        {
            if (imageUrl == null || imageUrl.Length == 0)
            {
                return BadRequest("Invalid image file.");
            }

            try
            {
                await _contentService.ChangeContentImage(contentId, imageUrl);
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
