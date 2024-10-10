namespace DTOs.Product
{
    public class ProductReviewDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<double> RentPrices { get; set; }
    }
}
