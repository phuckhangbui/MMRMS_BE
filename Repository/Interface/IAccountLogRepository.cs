using DTOs.Account;
using DTOs.Log;

namespace Repository.Interface
{
    public interface IAccountLogRepository
    {
        Task CreateFirstAccountLog(int accountId);
        Task<AccountLogDto> GetAccountLogByAccountId(int accountId);
        Task WriteNewAccountLogDetail(int accountId);
        Task<IEnumerable<LogDto>> GetLogs();
        Task<IEnumerable<LogDetailDto>> GetLogDetailsByLogId(int logId);
    }
}
