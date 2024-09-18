using DTOs.Content;
using DTOs.Promotion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        public async Task<ActionResult> GetPromotions()
        {
            try
            {
                var promotions = await _promotionService.GetPromotions();
                return Ok(promotions);
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
        public async Task<ActionResult> CreatePromotion([FromBody] PromotionCreateRequestDto promotionCreateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
                return BadRequest(errorMessages);
            }

            if (promotionCreateRequestDto.DateEnd < promotionCreateRequestDto.DateStart)
            {
                return BadRequest("Date End must be after Date Start");
            }

            try
            {
                await _promotionService.CreatePromotion(promotionCreateRequestDto);
                return Created("", promotionCreateRequestDto);
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
