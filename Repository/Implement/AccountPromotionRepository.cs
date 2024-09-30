using AutoMapper;
using DAO;
using DTOs.AccountPromotion;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class AccountPromotionRepository : IAccountPromotionRepository
    {
        private readonly IMapper _mapper;

        public AccountPromotionRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountPromotionDto>> GetAccountPromotionsForCustomer(int customerId)
        {
            var promotions = await AccountPromotionDao.Instance.GetPromotionsByCustomerId(customerId);

            if (!promotions.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<AccountPromotionDto>>(promotions);
            }

            return [];
        }
    }
}
