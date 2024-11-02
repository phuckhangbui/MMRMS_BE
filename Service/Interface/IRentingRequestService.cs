using DTOs.RentingRequest;

namespace Service.Interface
{
    public interface IRentingRequestService
    {
        Task<IEnumerable<RentingRequestDto>> GetRentingRequests(string? status);
        Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto);
        Task<RentingRequestDetailDto> GetRentingRequestDetail(string rentingRequestId);
        Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestMachineInRangeDto rentingRequestMachineInRangeDto);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId);
        Task<bool> CancelRentingRequest(string rentingRequestId);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequestsThatStillHaveContractNeedDelivery();
    }
}
