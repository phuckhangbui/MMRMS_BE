namespace BusinessObject
{
    public class ProductTerm
    {
        public int ProductTermId { get; set; }

        public int? ProductId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public virtual Product? Product { get; set; }
    }
}
