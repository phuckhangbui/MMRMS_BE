using AutoMapper;
using BusinessObject;
using DAO;
using DAO.Enum;
using DTOs.MembershipRank;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class MembershipRankRepository : IMembershipRankRepository
    {
        private readonly IMapper _mapper;

        public MembershipRankRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task ChangeMembershipRankStatus(int membershipRankId, string status)
        {
            var membershipRank = await MembershipRankDao.Instance.GetMembershipRankById(membershipRankId);
            if (membershipRank != null)
            {
                membershipRank.Status = status;
                await MembershipRankDao.Instance.UpdateAsync(membershipRank);
            }
        }

        public async Task CreateMembershipRank(MembershipRankRequestDto membershipRankRequestDto)
        {
            var membershipRank = _mapper.Map<MembershipRank>(membershipRankRequestDto);

            membershipRank.DateCreate = DateTime.Now;
            membershipRank.Status = MembershipRankStatusEnum.Inactive.ToString();

            await MembershipRankDao.Instance.CreateAsync(membershipRank);
        }

        public async Task DeleteMembershipRank(int membershipRankId)
        {
            var membershipRank = await MembershipRankDao.Instance.GetMembershipRankById(membershipRankId);
            if (membershipRank != null)
            {
                await MembershipRankDao.Instance.RemoveAsync(membershipRank);
            }
        }

        public async Task<MembershipRankDto?> GetMembershipRankById(int membershipRankId)
        {
            var membershipRank = await MembershipRankDao.Instance.GetMembershipRankById(membershipRankId);
            if (membershipRank != null)
            {
                return _mapper.Map<MembershipRankDto>(membershipRank);
            }

            return null;
        }

        public async Task<IEnumerable<MembershipRankDto>> GetMembershipRanks()
        {
            var membershipRanks = await MembershipRankDao.Instance.GetAllAsync();

            if (!membershipRanks.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<MembershipRankDto>>(membershipRanks);
            }

            return [];
        }

        public async Task UpdateMembershipRank(int membershipRankId, MembershipRankRequestDto membershipRankRequestDto)
        {
            var membershipRank = await MembershipRankDao.Instance.GetMembershipRankById(membershipRankId);
            if (membershipRank != null)
            {
                membershipRank.MembershipRankName = membershipRankRequestDto.MembershipRankName;
                membershipRank.DiscountPercentage = membershipRankRequestDto.DiscountPercentage;
                membershipRank.MoneySpent = membershipRankRequestDto.MoneySpent;
                membershipRank.Content = membershipRankRequestDto.Content;

                await MembershipRankDao.Instance.UpdateAsync(membershipRank);
            }
        }
    }
}
