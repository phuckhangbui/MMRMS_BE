namespace DTOs.MachineSerialNumber
{
    public class MachineSerialNumberDto
    {
        public string SerialNumber { get; set; } = null!;

        public double? ActualRentPrice { get; set; }

        public int? MachineId { get; set; }

        public string? Status { get; set; }

        public DateTime? DateCreate { get; set; }

        public int? RentTimeCounter { get; set; }
    }
}
