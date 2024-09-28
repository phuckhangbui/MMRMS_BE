using DTOs.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/test")]
    public class TestController : BaseApiController
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ISerialNumberProductRepository _serialNumberProductRepository;
        private readonly INotificationService _notificationService;
        private readonly IFirebaseMessagingService _firebaseMessagingService;

        public TestController(ICloudinaryService cloudinaryService, ISerialNumberProductRepository serialNumberProductRepository, IFirebaseMessagingService firebaseMessagingService, INotificationService notificationService)
        {
            _cloudinaryService = cloudinaryService;
            _serialNumberProductRepository = serialNumberProductRepository;
            _firebaseMessagingService = firebaseMessagingService;
            _notificationService = notificationService;
        }

        [HttpGet("{rentingRequestId}")]
        public async Task<ActionResult> GetSerialProductNumbersAvailableForRenting(string rentingRequestId)
        {
            try
            {
                var serialNumberProducts = await _serialNumberProductRepository.GetSerialProductNumbersAvailableForRenting(rentingRequestId);
                return Ok(serialNumberProducts);
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

        //for testing purpose
        [HttpPost("create-noti")]
        public async Task<ActionResult> Post([FromBody] CreateNotificationDto createNotificationDto)
        {
            try
            {
                await _notificationService.CreateNotification(createNotificationDto);
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

        [HttpPost("pushnoti")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            try
            {
                string response = await _firebaseMessagingService.SendPushNotification(request.RegistrationToken, request.Title, request.Body, request.Data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

        public class NotificationRequest
        {
            public string RegistrationToken { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public Dictionary<string, string> Data { get; set; }
        }
    }
}
