namespace DTOs.MaintainingTicket
{
    public class MaintaningTicketDto
    {
        public int MaintainingTicketId { get; set; }

        public int? EmployeeTaskId { get; set; }

        public int? EmployeeCreateId { get; set; }

        public int? ComponentId { get; set; }

        public string? InvoiceId { get; set; }

        public string? ProductSerialNumber { get; set; }

        public double? ComponentPrice { get; set; }

        public double? AdditionalFee { get; set; }

        public double? TotalAmount { get; set; }

        public int? Quantity { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateRepair { get; set; }

        public int? Type { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }
    }
}
