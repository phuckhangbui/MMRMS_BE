using DTOs.Notification;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Service.Exceptions;

namespace API.Controllers
{
    [Route("api/notifications")]

    public class NotificationController : BaseApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{receiveId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> Get([FromRoute] int receiveId)
        {
            try
            {
                var list = await _notificationService.GetNotificationsBaseOnReceiveId(receiveId);

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




        [HttpPut]
        public async Task<ActionResult> Put([FromQuery] int key)
        {
            try
            {
                await _notificationService.MarkNotificationAsRead(key);
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
