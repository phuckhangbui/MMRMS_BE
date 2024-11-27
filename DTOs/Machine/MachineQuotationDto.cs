namespace DTOs.Machine
{
    public class MachineQuotationDto
    {
        public int? MachineId { get; set; }

        public string? MachineName { get; set; }

        public string? Model { get; set; }

        public List<Dictionary<string, double>>? Quotation { get; set; }
    }
}
