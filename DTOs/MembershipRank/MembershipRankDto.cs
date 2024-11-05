using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MembershipRank
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

    public class MembershipRankDetailDto : MembershipRankDto
    {
        public List<MembershipRankLogDto> MembershipRankLogs { get; set; } = new List<MembershipRankLogDto>();
    }

    public class MembershipRankLogDto
    {
        public int MembershipRankLogId { get; set; }
        public int? MembershipRankId { get; set; }
        public int? AccountId { get; set; }
        public string? Action { get; set; }
        public DateTime? DateCreate { get; set; }
    }

    public class MembershipRankRequestDto
    {
        [Required(ErrorMessage = MessageConstant.MembershipRank.MembershipRankNameRequired)]
        public string? MembershipRankName { get; set; }

        [Required(ErrorMessage = MessageConstant.MembershipRank.MoneySpentRequired)]
        [Range(0, double.MaxValue, ErrorMessage = MessageConstant.MembershipRank.MoneySpentRange)]
        public double? MoneySpent { get; set; }

        [Required(ErrorMessage = MessageConstant.MembershipRank.DiscountPercentageRequired)]
        [Range(0, 100, ErrorMessage = MessageConstant.MembershipRank.DiscountPercentageRange)]
        public double? DiscountPercentage { get; set; }

        [Required(ErrorMessage = MessageConstant.MembershipRank.ContentRequired)]
        public string? Content { get; set; }
    }
}
