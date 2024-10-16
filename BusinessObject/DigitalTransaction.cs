namespace BusinessObject
{
    public class DigitalTransaction
    {
        public string DigitalTransactionId { get; set; }
        public string? InvoiceId { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankCode { get; set; }
        public string? BankName { get; set; }
        public double? Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? PayOsOrderId { get; set; }
        public virtual Invoice? Invoice { get; set; }
    }
}
