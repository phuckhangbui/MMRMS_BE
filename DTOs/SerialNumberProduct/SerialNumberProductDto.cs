namespace DTOs.SerialNumberProduct
{
    public class SerialNumberProductDto
    {
        public string SerialNumber { get; set; } = null!;

        public double? ActualRentPrice { get; set; }

        public int? ProductId { get; set; }

        public string? Status { get; set; }

        public DateTime? DateCreate { get; set; }

        public int? RentTimeCounter { get; set; }
    }
}
