using DTOs.RentingRequest;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class RentingService : IRentingService
    {
        private readonly IRentingRepository _rentingRepository;

        public RentingService(IRentingRepository rentingRepository)
        {
            _rentingRepository = rentingRepository;
        }

        public async Task<IEnumerable<RentingRequestDto>> GetAll()
        {
            return await _rentingRepository.GetRentingRequests();
        }
    }
}
