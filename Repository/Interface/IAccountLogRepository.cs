using DTOs.Log;

namespace Repository.Interface
{
    public interface IAccountLogRepository
    {
        Task<IEnumerable<LogDetailDto>> GetLogs();
        Task WriteNewAccountLogDetail(int accountId);
    }
}
