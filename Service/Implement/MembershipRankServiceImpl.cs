using Common;
using DAO.Enum;
using DTOs.MembershipRank;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class MembershipRankServiceImpl : IMembershipRankService
    {
        private readonly IMembershipRankRepository _membershipRankRepository;

        public MembershipRankServiceImpl(IMembershipRankRepository membershipRankRepository)
        {
            _membershipRankRepository = membershipRankRepository;
        }

        public async Task ChangeMembershipRankStatus(int membershipRankId, string status)
        {
            await CheckMembershipRankExist(membershipRankId);

            if (!Enum.IsDefined(typeof(MembershipRankStatusEnum), status))
            {
                throw new ServiceException(MessageConstant.InvalidStatusValue);
            }

            await _membershipRankRepository.ChangeMembershipRankStatus(membershipRankId, status);
        }

        public async Task CreateMembershipRank(MembershipRankRequestDto membershipRankRequestDto)
        {
            await _membershipRankRepository.CreateMembershipRank(membershipRankRequestDto);
        }

        public async Task DeleteMembershipRank(int membershipRankId)
        {
            await CheckMembershipRankExist(membershipRankId);

            await _membershipRankRepository.DeleteMembershipRank(membershipRankId);
        }

        public async Task<MembershipRankDto> GetMembershipRankById(int membershipRankId)
        {
            var membershipRank = await CheckMembershipRankExist(membershipRankId);

            return membershipRank;
        }

        public async Task<IEnumerable<MembershipRankDto>> GetMembershipRanks()
        {
            var membershipRanks = await _membershipRankRepository.GetMembershipRanks();

            if (membershipRanks.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.MembershipRank.MembershipRanksEmpty);
            }

            return membershipRanks;
        }

        public async Task UpdateMembershipRank(int membershipRankId, MembershipRankRequestDto membershipRankRequestDto)
        {
            await CheckMembershipRankExist(membershipRankId);

            await _membershipRankRepository.UpdateMembershipRank(membershipRankId, membershipRankRequestDto);
        }

        private async Task<MembershipRankDto> CheckMembershipRankExist(int membershipRankId)
        {
            var membershipRank = await _membershipRankRepository.GetMembershipRankById(membershipRankId);

            if (membershipRank == null)
            {
                throw new ServiceException(MessageConstant.MembershipRank.MembershipRankNotFound);
            }

            return membershipRank;
        }
    }
}
