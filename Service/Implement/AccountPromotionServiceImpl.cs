//using Common;
//using DTOs.AccountPromotion;
//using Microsoft.IdentityModel.Tokens;
//using Repository.Interface;
//using Service.Exceptions;
//using Service.Interface;

//namespace Service.Implement
//{
//    public class AccountPromotionServiceImpl : IAccountPromotionService
//    {
//        private readonly IAccountPromotionRepository _accountPromotionRepository;

//        public AccountPromotionServiceImpl(IAccountPromotionRepository accountPromotionRepository)
//        {
//            _accountPromotionRepository = accountPromotionRepository;
//        }

//        public async Task<IEnumerable<AccountPromotionDto>> GetAccountPromotionsForCustomer(int customerId)
//        {
//            var promotions = await _accountPromotionRepository.GetAccountPromotionsForCustomer(customerId);

//            if (promotions.IsNullOrEmpty())
//            {
//                throw new ServiceException(MessageConstant.AccountPromotion.AccountPromotionListEmpty);
//            }

//            return promotions;
//        }
//    }
//}
