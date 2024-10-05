using DTOs.RentingService;

namespace Repository.Interface
{
    public interface IRentingServiceRepository
    {
        Task<IEnumerable<RentingServiceDto>> GetRentingServices();
        Task CreateRentingService(RentingServiceRequestDto rentingServiceRequestDto);
        Task<RentingServiceDto?> GetRentingServiceById(int rentingServiceId);
        Task UpdateRentingService(int rentingServiceId, RentingServiceRequestDto rentingServiceRequestDto);
        Task<bool> CanDeleteRentingService(int rentingServiceId);
        Task DeleteRentingService(int rentingServiceId);
    }
}
