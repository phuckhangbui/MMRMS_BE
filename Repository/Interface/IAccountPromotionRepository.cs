using DTOs.AccountPromotion;

namespace Repository.Interface
{
    public interface IAccountPromotionRepository
    {
        Task<IEnumerable<AccountPromotionDto>> GetAccountPromotionsForCustomer(int customerId);
    }
}
