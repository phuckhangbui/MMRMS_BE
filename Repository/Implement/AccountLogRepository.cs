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

        public async Task<IEnumerable<LogDetailDto>> GetLogs()
        {
            var logs = await AccountLogDetailDao.Instance.GetLogs();

            return _mapper.Map<IEnumerable<LogDetailDto>>(logs);
        }

        public async Task WriteNewAccountLogDetail(int accountId)
        {
            var detail = new LogDetail
            {
                AccountId = accountId,
                Action = "Login",
                DateCreate = DateTime.Now,
            };

            await AccountLogDetailDao.Instance.CreateAsync(detail);
        }
    }
}
