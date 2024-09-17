﻿using DTOs.Component;
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
            try
            {
                await _componentService.CreateComponet(createComponentDto);
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
