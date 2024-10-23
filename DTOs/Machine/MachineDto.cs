namespace DTOs.Machine
{
    public class MachineDto
    {
        public int MachineId { get; set; }

        public string? MachineName { get; set; }

        public string? CategoryName { get; set; }

        public double? RentPrice { get; set; }

        public double? ShipPricePerKm { get; set; }

        public double? MachinePrice { get; set; }

        public int? Quantity { get; set; }

        public string? Model { get; set; }
        public string? Description { get; set; }

        public string? Origin { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }

        public IEnumerable<MachineImageDto>? MachineImageList { get; set; }
    }
}
