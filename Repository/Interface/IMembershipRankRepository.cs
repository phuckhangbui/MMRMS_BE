﻿using DTOs.MembershipRank;

namespace Repository.Interface
{
    public interface IMembershipRankRepository
    {
        Task<IEnumerable<MembershipRankDto>> GetMembershipRanks();
        Task<MembershipRankDto?> GetMembershipRankById(int membershipRankId);
        Task CreateMembershipRank(MembershipRankRequestDto membershipRankRequestDto);
        Task UpdateMembershipRank(int membershipRankId, MembershipRankRequestDto membershipRankRequestDto);
        Task DeleteMembershipRank(int membershipRankId);
        Task ChangeMembershipRankStatus(int membershipRankId, string status);
        Task<MembershipRankDetailDto?> GetMembershipRankForCustomer(int customerId);
        Task AddMembershipRankLog(int customerId, int membershipRankId, string action);
    }
}
