namespace BusinessObject
{
    public partial class MembershipRank
    {
        public int MembershipRankId { get; set; }
        public string? MembershipRankName { get; set; }
        public double? MoneySpent { get; set; }
        public double? DiscountPercentage { get; set; }
        public string? Content { get; set; }
        public DateTime? DateCreate { get; set; }
        public string? Status { get; set; }
        public virtual ICollection<Account>? Accounts { get; set; } = new List<Account>();
        public virtual ICollection<MembershipRankLog>? MembershipRankLogs { get; set; } = new List<MembershipRankLog>();
    }
}
