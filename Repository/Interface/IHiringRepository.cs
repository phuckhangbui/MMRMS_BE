namespace Repository.Interface
{
    public interface IHiringRepository
    {
        Task<bool> CheckHiringRequestValidToRent(string hiringRequestId);
    }
}
