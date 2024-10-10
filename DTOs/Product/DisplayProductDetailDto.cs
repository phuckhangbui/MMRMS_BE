namespace DTOs.Product
{
    public class DisplayProductDetailDto
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? CategoryName { get; set; }

        public double? RentPrice { get; set; }

        public double? ProductPrice { get; set; }

        public int? Quantity { get; set; }

        public string? Model { get; set; }

        public string? Origin { get; set; }

        public int? CategoryId { get; set; }

        public string? Description { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }


        public IEnumerable<ProductAttributeDto>? ProductAttributeList { get; set; }

        public IEnumerable<ProductImageDto>? ProductImageList { get; set; }

        public IEnumerable<ComponentProductDto>? ComponentProductList { get; set; }

        public IEnumerable<ProductTermDto>? ProductTermList { get; set; }

        public List<double> RentPrices { get; set; }
    }

    public class ProductAttributeDto
    {
        public int ProductAttributeId { get; set; }

        public int? ProductId { get; set; }

        public string? AttributeName { get; set; }

        public string? Specifications { get; set; }

        public string? Unit { get; set; }

    }

    public class ProductTermDto
    {
        public int ProductTermId { get; set; }

        public int? ProductId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

    }
}
