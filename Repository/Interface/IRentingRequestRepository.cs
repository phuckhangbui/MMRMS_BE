using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IRentingRequestRepository
    {
        //Task<bool> CheckRentingRequestValidToRent(string rentingRequestId);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequests();
        Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto);
        Task<RentingRequestDetailDto?> GetRentingRequestDetailById(string rentingRequestId);
        Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestProductInRangeDto rentingRequestProductInRangeDto);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId);
        Task<bool> IsRentingRequestValidToCancel(string rentingRequestId);
        Task<bool> CancelRentingRequest(string rentingRequestId);
    }
}
