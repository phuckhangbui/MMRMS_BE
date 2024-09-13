namespace DTOs.Product
{
    public class ProductImageDto
    {
        public int ProductImageId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductImageUrl { get; set; }
        public bool? IsThumbnail { get; set; }
    }
}
