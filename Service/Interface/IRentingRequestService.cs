using DTOs.RentingRequest;

namespace Service.Interface
{
    public interface IRentingRequestService
    {
        Task<IEnumerable<RentingRequestDto>> GetAll();
        Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto);
        Task<RentingRequestDetailDto> GetRentingRequestDetailById(string rentingRequestId);
        Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestProductInRangeDto rentingRequestProductInRangeDto);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId);
    }
}
