namespace DTOs.SerialNumberProduct
{
    public class SerialProductNumberDto
    {
        public string SerialNumber { get; set; } = null!;

        public int? ProductId { get; set; }

        public string? Status { get; set; }

        public DateTime? DateCreate { get; set; }

        public int? RentTimeCounter { get; set; }

        public bool? IsDelete { get; set; }
    }
}
