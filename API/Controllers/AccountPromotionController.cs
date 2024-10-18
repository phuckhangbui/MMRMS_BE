//using DTOs.AccountPromotion;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Service.Exceptions;
//using Service.Interface;

//namespace API.Controllers
//{
//    [Route("api/account-promotions")]
//    public class AccountPromotionController : BaseApiController
//    {
//        private readonly IAccountPromotionService _accountPromotionService;

//        public AccountPromotionController(IAccountPromotionService accountPromotionService)
//        {
//            _accountPromotionService = accountPromotionService;
//        }

//        [HttpGet]
//        [Authorize(policy: "Customer")]
//        public async Task<ActionResult<List<AccountPromotionDto>>> GetAccountPromotionsForCustomer()
//        {
//            try
//            {
//                int customerId = GetLoginAccountId();
//                var promotions = await _accountPromotionService.GetAccountPromotionsForCustomer(customerId);
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
//    }
//}
