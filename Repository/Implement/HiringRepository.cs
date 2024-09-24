using AutoMapper;
using BusinessObject;
using DAO;
using DAO.Enum;
using DTOs.HiringRequest;
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

        public async Task<IEnumerable<HiringRequestDto>> GetHiringRequests()
        {
            IEnumerable<HiringRequest> hiringRequests = await HiringRequestDao.Instance.GetHiringRequests();

            return _mapper.Map<IEnumerable<HiringRequestDto>>(hiringRequests);
        }
    }
}
