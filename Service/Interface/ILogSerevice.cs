using DTOs.LogDto;

namespace Service.Interface
{
    public interface ILogSerevice
    {
        Task<IEnumerable<LogDetailDto>> GetLogs();
    }
}
