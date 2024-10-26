namespace DAO
{
    public interface IBackground
    {
        void ScheduleCancelRentingRequestJob(string rentingRequestId);
        void ScheduleCompleteContractOnTimeJob(string contractId, TimeSpan delayToStart);
    }
}
