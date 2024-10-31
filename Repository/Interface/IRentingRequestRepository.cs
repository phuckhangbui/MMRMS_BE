using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IRentingRequestRepository
    {
        //Task<bool> CheckRentingRequestValidToRent(string rentingRequestId);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequests();
        Task<RentingRequestDto> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto);
        Task<RentingRequestDetailDto?> GetRentingRequestDetailById(string rentingRequestId);
        Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestMachineInRangeDto rentingRequestMachineInRangeDto);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId);
        Task<bool> IsRentingRequestValidToCancel(string rentingRequestId);
        Task<bool> CancelRentingRequest(string rentingRequestId);
        Task<RentingRequestDto> GetRentingRequest(string rentingRequestId);
        Task UpdateRentingRequest(RentingRequestDto rentingRequest);
    }
}
