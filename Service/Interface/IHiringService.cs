using DTOs.RentingRequest;

namespace Service.Interface
{
    public interface IRentingService
    {
        Task<IEnumerable<RentingRequestDto>> GetAll();
    }
}
