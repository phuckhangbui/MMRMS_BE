using DTOs.Log;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class LogSereviceImpl : ILogSerevice
    {
        private readonly IAccountLogRepository _accountLogRepository;

        public LogSereviceImpl(IAccountLogRepository accountLogRepository)
        {
            _accountLogRepository = accountLogRepository;
        }

        public async Task<IEnumerable<LogDetailDto>> GetLogs()
        {
            return await _accountLogRepository.GetLogs();
        }
    }
}
