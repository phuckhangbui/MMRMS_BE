namespace DTOs
{
    public class InvoiceDto
    {
        public string InvoiceId { get; set; } = null!;

        public string? InvoiceCode { get; set; }

        public int? AccountPaidId { get; set; }

        public string? AccountPaidName { get; set; }

        public string? ContractId { get; set; }

        public string? PaymentMethod { get; set; }

        public double? Amount { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public string? Note { get; set; }
    }
}
