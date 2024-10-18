//using DTOs.Promotion;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Service.Exceptions;
//using Service.Interface;

//namespace API.Controllers
//{
//    [Route("api/promotions")]
//    public class PromotionController : BaseApiController
//    {
//        private readonly IPromotionService _promotionService;

//        public PromotionController(IPromotionService promotionService)
//        {
//            _promotionService = promotionService;
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetPromotions()
//        {
//            try
//            {
//                var promotions = await _promotionService.GetPromotions();
//                return Ok(promotions);
//            }
//            catch (ServiceException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpGet("{promotionId}")]
//        public async Task<ActionResult<PromotionDto>> GetPromotionDetailById(int promotionId)
//        {
//            try
//            {
//                var promotion = await _promotionService.GetPromotionById(promotionId);
//                return Ok(promotion);
//            }
//            catch (ServiceException ex)
//            {
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpPost]
//        [Authorize(Policy = "Manager")]
//        public async Task<ActionResult> CreatePromotion([FromBody] PromotionRequestDto promotionRequestDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
//                return BadRequest(errorMessages);
//            }

//            if (promotionRequestDto.DateEnd < promotionRequestDto.DateStart)
//            {
//                return BadRequest("Date End must be after Date Start");
//            }

//            try
//            {
//                await _promotionService.CreatePromotion(promotionRequestDto);
//                return Created("", promotionRequestDto);
//            }
//            catch (ServiceException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpPut("{promotionId}")]
//        [Authorize(Policy = "Manager")]
//        public async Task<ActionResult> UpdatePromotion(int promotionId, [FromBody] PromotionRequestDto promotionRequestDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                string errorMessages = ModelStateValidation.GetValidationErrors(ModelState);
//                return BadRequest(errorMessages);
//            }

//            if (promotionRequestDto.DateEnd < promotionRequestDto.DateStart)
//            {
//                return BadRequest("Date End must be after Date Start");
//            }

//            try
//            {
//                await _promotionService.UpdatePromotion(promotionId, promotionRequestDto);
//                return NoContent();
//            }
//            catch (ServiceException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpDelete("{promotionId}")]
//        [Authorize(Policy = "Manager")]
//        public async Task<ActionResult> DeletePromotion(int promotionId)
//        {
//            try
//            {
//                await _promotionService.DeletePromotion(promotionId);
//                return NoContent();
//            }
//            catch (ServiceException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }
//    }
//}
