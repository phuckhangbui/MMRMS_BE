using DTOs.HiringRequest;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class HiringService : IHiringService
    {
        private readonly IHiringRepository _hiringRepository;

        public HiringService(IHiringRepository hiringRepository)
        {
            _hiringRepository = hiringRepository;
        }

        public async Task<IEnumerable<HiringRequestDto>> GetAll()
        {
            return await _hiringRepository.GetHiringRequests();
        }
    }
}
