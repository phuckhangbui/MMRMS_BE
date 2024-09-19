using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class MembershipRankDto
    {
        public int MembershipRankId { get; set; }
        public string? MembershipRankName { get; set; }
        public double? MoneySpent { get; set; }
        public double? DiscountPercentage { get; set; }
        public string? Content { get; set; }
        public DateTime? DateCreate { get; set; }
        public string? Status { get; set; }
    }

    public class MembershipRankRequestDto
    {
        [Required(ErrorMessage = "MembershipRankName is required")]
        public string? MembershipRankName { get; set; }

        [Required(ErrorMessage = "MoneySpent is required")]
        [Range(0, double.MaxValue, ErrorMessage = "MoneySpent must be a positive number")]
        public double? MoneySpent { get; set; }

        [Required(ErrorMessage = "DiscountPercentage is required")]
        [Range(0, 100, ErrorMessage = "DiscountPercentage must be between 0 and 100")]
        public double? DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }
    }
}
