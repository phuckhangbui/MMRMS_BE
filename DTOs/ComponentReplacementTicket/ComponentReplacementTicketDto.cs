namespace DTOs.ComponentReplacementTicket
{
    public class ComponentReplacementTicketDto
    {
        public string ComponentReplacementTicketId { get; set; }

        public int? EmployeeCreateId { get; set; }

        public string? EmployeeCreateName { get; set; }

        public int? MachineTaskCreateId { get; set; }

        public string? ContractId { get; set; }

        public int? ComponentId { get; set; }

        public string? ComponentName { get; set; }

        public string? InvoiceId { get; set; }

        public int? MachineSerialNumberComponentId { get; set; }

        public string? SerialNumber { get; set; }

        public string? MachineName { get; set; }

        public double? ComponentPrice { get; set; }

        public double? AdditionalFee { get; set; }

        public double? TotalAmount { get; set; }

        public int? Quantity { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateRepair { get; set; }

        public string? Type { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }
    }
}
