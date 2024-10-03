using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IRentingRepository
    {
        Task<bool> CheckRentingRequestValidToRent(string rentingRequestId);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequests();
        Task CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto);
        Task<RentingRequestDetailDto?> GetRentingRequestDetailById(string rentingRequestId);
        Task<RentingRequestInitDataDto> GetRentingRequestInitData(int customerId, List<int> productIds);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId);
    }
}
