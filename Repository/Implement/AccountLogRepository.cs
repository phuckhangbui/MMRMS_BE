using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.Account;
using Repository.Interface;

namespace Repository.Implement
{
    public class AccountLogRepository : IAccountLogRepository
    {
        private readonly IMapper _mapper;

        public AccountLogRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreateFirstAccountLog(int accountId)
        {
            var now = DateTime.Now;
            var log = new Log
            {
                AccountLogId = accountId,
                DateCreate = now,
                DateUpdate = now,
            };

            var logDetail = new LogDetail
            {
                LogId = log.LogId,
                Action = "First Login",
                DateCreate = now,
            };

            List<LogDetail> details = [logDetail];

            log.LogDetails = details;

            await AccountLogDao.Instance.CreateAsync(log);
        }

        public async Task<AccountLogDto> GetAccountLogByAccountId(int accountId)
        {
            var accountLog = await AccountLogDao.Instance.GetAccountLogByAccountId(accountId);

            return _mapper.Map<AccountLogDto>(accountLog);
        }

        public async Task WriteNewAccountLogDetail(int accountId)
        {
            var accountLog = await AccountLogDao.Instance.GetAccountLogByAccountId(accountId);

            var now = DateTime.Now;

            accountLog.DateUpdate = now;

            var detail = new LogDetail
            {
                LogId = accountLog.LogId,
                Action = "Login",
                DateCreate = now,
            };

            accountLog.LogDetails.Add(detail);

            await AccountLogDao.Instance.UpdateAsync(accountLog);
        }
    }
}
