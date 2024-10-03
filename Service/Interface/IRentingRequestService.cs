using DTOs.RentingRequest;

namespace Service.Interface
{
    public interface IRentingRequestService
    {
        Task<IEnumerable<RentingRequestDto>> GetAll();
        Task CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto);
        Task<RentingRequestDetailDto> GetRentingRequestDetailById(string rentingRequestId);
        Task<RentingRequestInitDataDto> GetRentingRequestInitData(int customerId, List<int> productIds);
        Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId);
    }
}
