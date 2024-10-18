namespace Repository
{
    public interface IBackground
    {
        void ScheduleCancelRentingRequestJob(string rentingRequestId);
    }
}
