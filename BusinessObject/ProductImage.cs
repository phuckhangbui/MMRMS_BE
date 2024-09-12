namespace BusinessObject
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductImageUrl { get; set; }
        public bool? IsThumbnail { get; set; }
        public virtual Product? Product { get; set; }
    }
}
