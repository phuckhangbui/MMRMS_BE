namespace DTOs.SerialNumberProduct
{
    public class ProductComponentStatusDto
    {
        public int ProductComponentStatusId { get; set; }

        public string? SerialNumber { get; set; }

        public int? ComponentId { get; set; }

        public string? ComponentName { get; set; }

        public int? Quantity { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }
    }
}
