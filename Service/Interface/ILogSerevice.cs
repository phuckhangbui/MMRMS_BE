using DTOs.Log;

namespace Service.Interface
{
    public interface ILogSerevice
    {
        Task<IEnumerable<LogDto>> GetLogs();
        Task<IEnumerable<LogDetailDto>> GetLogDetailsByLogId(int logId);
    }
}
