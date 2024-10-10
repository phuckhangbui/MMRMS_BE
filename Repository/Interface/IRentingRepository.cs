using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IRentingRepository
    {
        Task<bool> CheckRentingRequestValidToRent(string rentingRequestId);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequests();
        Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto);
        Task<RentingRequestDetailDto?> GetRentingRequestDetailById(string rentingRequestId);
        Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestProductInRangeDto rentingRequestProductInRangeDto);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId);
    }
}
