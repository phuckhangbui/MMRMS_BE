﻿using DTOs.Notification;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

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
    }
}
