namespace BusinessObject
{
    public partial class MembershipRankLog
    {
        public int MembershipRankLogId { get; set; }

        public int? MembershipRankId { get; set; }

        public int? AccountId { get; set; }

        public string? Action { get; set; }

        public DateTime? DateCreate { get; set; }

        public virtual MembershipRank? MembershipRank { get; set; }

        public virtual Account? Account { get; set; }
    }
}
