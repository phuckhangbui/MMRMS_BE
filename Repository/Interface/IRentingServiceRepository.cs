using DTOs.RentingService;

namespace Repository.Interface
{
    public interface IRentingServiceRepository
    {
        Task<IEnumerable<RentingServiceDto>> GetRentingServices();
        Task CreateRentingService(RentingServiceRequestDto rentingServiceRequestDto);
        Task UpdateRentingService(int rentingServiceId, RentingServiceRequestDto rentingServiceRequestDto);
    }
}
