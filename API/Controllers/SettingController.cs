using DTOs.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/settings")]
    public class SettingsController : BaseApiController
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SettingDto>>> GetSettings()
        {
            try
            {
                var settings = await _settingsService.GetSettingsAsync();
                return Ok(settings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> UpdateSetting(UpdateSettingDto updateSettingDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                await _settingsService.UpdateSettingAsync(updateSettingDto);
                return Ok("Setting updated successfully.");
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
