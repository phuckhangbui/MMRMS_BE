using AutoMapper;
using DAO;
using DAO.Enum;
using Repository.Interface;

namespace Repository.Implement
{
    public class HiringRepository : IHiringRepository
    {
        private readonly IMapper _mapper;

        public HiringRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CheckHiringRequestValidToRent(string hiringRequestId)
        {
            var hiringRequest = await HiringRequestDao.Instance.GetHiringRequestByIdAndStatus(hiringRequestId, HiringRequestStatusEnum.Approved.ToString());
            return hiringRequest != null;
        }
    }
}
