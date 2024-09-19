using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/test")]
    public class TestController : BaseApiController
    {
        private readonly ICloudinaryService _cloudinaryService;

        public TestController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet("datetime")]
        public async Task<ActionResult> GetDatetime()
        {
            return Ok(DateTime.Now);
        }

        [Authorize]
        [HttpGet("token/expire")]
        public async Task<ActionResult> CheckIsTokenExpire()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("images")]
        public async Task<ActionResult> UploadImage([FromForm] FileViewModel fileviewmodel)
        {
            try
            {
                if (fileviewmodel == null || fileviewmodel.File.Length == 0)
                {
                    return BadRequest("Invalid image file.");
                }

                var images = new List<string>();

                var result = await _cloudinaryService.AddPhotoAsync(fileviewmodel.File);
                if (result.Error != null)
                {
                    //throw new ServiceException("Error uploading image to Cloudinary: " + result.Error.Message);
                }

                string imageUrl = result.SecureUrl.AbsoluteUri;

                images.Add(imageUrl);

                return Ok(result);
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

        public class FileViewModel
        {
            public string? Name { get; set; }
            public IFormFile File { get; set; }
        }
    }
}
