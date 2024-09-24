using DTOs.HiringRequest;

namespace Repository.Interface
{
    public interface IHiringRepository
    {
        Task<bool> CheckHiringRequestValidToRent(string hiringRequestId);

        Task<IEnumerable<HiringRequestDto>> GetHiringRequests();
    }
}
