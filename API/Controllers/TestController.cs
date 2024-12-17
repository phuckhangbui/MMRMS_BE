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
        private readonly INotificationService _notificationService;
        private readonly IFirebaseMessagingService _firebaseMessagingService;
        private readonly IMembershipRankService _membershipRankService;
        private readonly IPayOSService _payOSService;
        private readonly ILogger<TestController> _logger;
        private readonly IMachineSerialNumberService _machineSerialNumberService;

        public TestController(ILogger<TestController> logger,
            ICloudinaryService cloudinaryService,
            IFirebaseMessagingService firebaseMessagingService,
            INotificationService notificationService,
            IMembershipRankService membershipRankService,
            IPayOSService payOSService,
            IMachineSerialNumberService machineSerialNumberService)
        {
            _cloudinaryService = cloudinaryService;
            _firebaseMessagingService = firebaseMessagingService;
            _notificationService = notificationService;
            _membershipRankService = membershipRankService;
            _logger = logger;
            _payOSService = payOSService;
            _machineSerialNumberService = machineSerialNumberService;
        }

        [HttpPost("serial-number")]
        public async Task<IActionResult> UpdateSerialNumber()
        {
            try
            {
                await _machineSerialNumberService.UpdateRentDaysCounterMachineSerialNumber("TESTHUIA5", 400);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the customer rank.", details = ex.Message });
            }
        }

        [HttpPost("update-customer-rank")]
        public async Task<IActionResult> UpdateCustomerRank(int customerId, double amount)
        {
            try
            {
                await _membershipRankService.UpdateMembershipRankForCustomer(customerId, amount);
                return Ok(new { message = "Customer membership rank updated successfully." });
            }
            catch (ServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the customer rank.", details = ex.Message });
            }
        }

        [HttpPost("create-payment-url")]
        public async Task<IActionResult> CreatePaymentUrl()
        {
            try
            {
                var url = await _payOSService.CreatePaymentLink("TEST", 123456, 1000, "test", "test");
                return Ok(url);
            }
            catch (ServiceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the customer rank.", details = ex.Message });
            }
        }

        [HttpGet("datetime")]
        public async Task<ActionResult> GetDatetime()
        {
            _logger.LogInformation("Datetime endpoint hit at {Time}", DateTime.Now);

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

        [HttpPost("images/1-image-string")]
        public async Task<ActionResult> UploadImage([FromBody] Image image)
        {
            try
            {
                string imageUrl = await _cloudinaryService.UploadImageToCloudinary(image.Base64Image[0]);

                return Ok(new { Url = imageUrl });
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

        [HttpPost("images/multi-image-string")]
        public async Task<ActionResult> UploadImages([FromBody] Image image)
        {
            try
            {
                string[] imageUrls = await _cloudinaryService.UploadImageToCloudinary(image.Base64Image);

                return Ok(new { Url = imageUrls });
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

        public class Image
        {
            public string[] Base64Image { get; set; }
        }
    }
}
