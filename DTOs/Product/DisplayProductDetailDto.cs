namespace DTOs.Product
{
    public class DisplayProductDetailDto
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? CategoryName { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }

        public string? Model { get; set; }

        public string? Origin { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }

        public bool? IsDelete { get; set; }

    }

    public class ProductComponentDetailDto
    {
        public int ProductDetailId { get; set; }

        public int? ProductId { get; set; }

        public int? ComponentId { get; set; }

        public DateTime? DateCreate { get; set; }

        public bool? IsDelete { get; set; }
    }
}
