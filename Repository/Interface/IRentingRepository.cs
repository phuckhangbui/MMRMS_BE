using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IRentingRepository
    {
        Task<bool> CheckRentingRequestValidToRent(string rentingRequestId);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequests();
        Task CreateRentingRequest(NewRentingRequestDto newRentingRequestDto);
    }
}
