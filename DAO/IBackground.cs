namespace DAO
{
    public interface IBackground
    {
        void CancelRentingRequestJob(string rentingRequestId);
        void CompleteContractOnTimeJob(string contractId, TimeSpan delayToStart);
        void GenerateInvoiceJob(int nextContractPaymentId, TimeSpan delayToStart);
    }
}
