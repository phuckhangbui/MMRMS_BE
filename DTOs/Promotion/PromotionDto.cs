using DTOs.Validation;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Promotion
{
    public class PromotionDto
    {
        public int PromotionId { get; set; }
        public string? DiscountTypeName { get; set; }
        public double? DiscountPercentage { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateCreate { get; set; }
        public string? Status { get; set; }
    }

    public class PromotionCreateRequestDto
    {
        [Required(ErrorMessage = "Discount Type Name is required")]
        public string? DiscountTypeName { get; set; }

        [Required(ErrorMessage = "Discount Percentage is required")]
        [Range(0, 100, ErrorMessage = "Discount Percentage must be between 0 and 100")]
        public double? DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Date Start is required")]
        [FutureOrPresentDate(ErrorMessage = "Date Start must be today or in the future")]
        public DateTime? DateStart { get; set; }

        [Required(ErrorMessage = "Date End is required")]
        public DateTime? DateEnd { get; set; }
    }
}
