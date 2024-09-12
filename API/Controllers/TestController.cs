﻿using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/abcTest")]
    public class TestController : BaseApiController
    {
        [HttpGet("/datetime")]
        public async Task<ActionResult> GetDatetime([FromQuery] int? role)
        {
            return Ok(DateTime.Now);
        }
    }
}
