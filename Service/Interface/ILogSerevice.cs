using DTOs.Log;

namespace Service.Interface
{
    public interface ILogSerevice
    {
        Task<IEnumerable<LogDetailDto>> GetLogs();
    }
}
