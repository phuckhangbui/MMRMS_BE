namespace DTOs.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? CategoryName { get; set; }

        public double? RentPrice { get; set; }

        public double? ProductPrice { get; set; }

        public int? Quantity { get; set; }

        public string? Model { get; set; }
        public string? Description { get; set; }

        public string? Origin { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }

        public IEnumerable<ProductImageDto>? ProductImageList { get; set; }
    }
}
