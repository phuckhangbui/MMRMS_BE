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

        public IEnumerable<ProductAttributeDto>? ProductAttributeList { get; set; }

        public IEnumerable<ProductImageDto>? ProductImageList { get; set; }

        public IEnumerable<ComponentProductDto>? ComponentProductList { get; set; }

    }

    public class ComponentProductDto
    {
        public int ComponentProductId { get; set; }

        public string? ComponentName { get; set; }

        public int ProductId { get; set; }

        public int? Quantity { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }
    }

    public class ProductAttributeDto
    {
        public int ProductAttributeId { get; set; }

        public int? ProductId { get; set; }

        public string? AttributeName { get; set; }

        public string? Specifications { get; set; }

        public string? Unit { get; set; }

    }
}
