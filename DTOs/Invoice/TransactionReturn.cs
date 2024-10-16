namespace DTOs.Invoice
{
    public class TransactionReturn
    {
        public string? Reference { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankCode { get; set; }
        public string? BankName { get; set; }
        public double? Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
