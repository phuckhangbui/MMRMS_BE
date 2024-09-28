using AutoMapper;
using BusinessObject;
using DAO;
using DAO.Enum;
using DTOs.RentingRequest;
using Repository.Interface;

namespace Repository.Implement
{
    public class RentingRepository : IRentingRepository
    {
        private readonly IMapper _mapper;

        public RentingRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CheckRentingRequestValidToRent(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestByIdAndStatus(rentingRequestId, RentingRequestStatusEnum.Approved.ToString());
            return rentingRequest != null;
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequests()
        {
            IEnumerable<RentingRequest> rentingRequests = await RentingRequestDao.Instance.GetRentingRequests();

            return _mapper.Map<IEnumerable<RentingRequestDto>>(rentingRequests);
        }
    }
}
