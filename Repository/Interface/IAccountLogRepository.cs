using DTOs.Log;

namespace Repository.Interface
{
    public interface IAccountLogRepository
    {
        Task<LogDetailDto> GetAccountLogByAccountId(int accountId);
        Task WriteNewAccountLogDetail(int accountId);
    }
}
