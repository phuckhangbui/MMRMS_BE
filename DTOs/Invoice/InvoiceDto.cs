namespace DTOs.Invoice
{
    public class InvoiceDto
    {
        public string InvoiceId { get; set; } = null!;

        public int? AccountPaidId { get; set; }

        public string? AccountPaidName { get; set; }

        public string? ComponentReplacementTicketId { get; set; }

        public string? DigitalTransactionId { get; set; }

        public string? PaymentMethod { get; set; }

        public double? Amount { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DatePaid { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public string? Note { get; set; }

        public string? PayOsOrderId { get; set; }

    }
}
