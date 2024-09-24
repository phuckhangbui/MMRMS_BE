using DTOs.HiringRequest;

namespace Service.Interface
{
    public interface IHiringService
    {
        Task<IEnumerable<HiringRequestDto>> GetAll();
    }
}
