using DTOs.RentingRequest;

namespace Service.Interface
{
    public interface IRentingRequestService
    {
        Task<IEnumerable<RentingRequestDto>> GetAll();
        Task CreateRentingRequest(NewRentingRequestDto newRentingRequestDto);
        Task<RentingRequestDetailDto> GetRentingRequestDetailById(string rentingRequestId);
    }
}
