using DTOs.Account;

namespace Repository.Interface
{
    public interface IAccountLogRepository
    {
        Task CreateFirstAccountLog(int accountId);
        Task<AccountLogDto> GetAccountLogByAccountId(int accountId);
        Task WriteNewAccountLogDetail(int accountId);
    }
}
