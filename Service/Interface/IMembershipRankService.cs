using DTOs.MembershipRank;

namespace Service.Interface
{
    public interface IMembershipRankService
    {
        Task<IEnumerable<MembershipRankDto>> GetMembershipRanks();
        Task<MembershipRankDto> GetMembershipRankById(int membershipRankId);
        Task CreateMembershipRank(MembershipRankRequestDto membershipRankRequestDto);
        Task UpdateMembershipRank(int membershipRankId, MembershipRankRequestDto membershipRankRequestDto);
        Task DeleteMembershipRank(int membershipRankId);
        Task ChangeMembershipRankStatus(int membershipRankId, string status);
        Task<MembershipRankDetailDto> GetMembershipRankForCustomer(int customerId);
        Task UpdateMembershipRankForCustomer(int customerId, double amount);
    }
}
