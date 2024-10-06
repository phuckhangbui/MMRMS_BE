using AutoMapper;
using BusinessObject;
using DAO;
using DTOs.Log;
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


        public async Task<LogDetailDto> GetAccountLogByAccountId(int accountId)
        {
            var accountLog = await AccountLogDetailDao.Instance.GetLogDetails(accountId);

            return _mapper.Map<LogDetailDto>(accountLog);
        }

        public async Task WriteNewAccountLogDetail(int accountId)
        {
            var detail = new LogDetail
            {
                AccountId = accountId,
                Action = "Login",
                DateCreate = DateTime.Now,
            };

            await AccountLogDetailDao.Instance.UpdateAsync(detail);
        }
    }
}
