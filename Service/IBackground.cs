namespace Service
{
    public interface IBackground
    {
        void CancelRentingRequestJob(string rentingRequestId);
        void CompleteContractOnTimeJob(string contractId, TimeSpan delayToStart);
        void ProcessExtendContractJob(string contractId, TimeSpan delayToStart);
    }
}
