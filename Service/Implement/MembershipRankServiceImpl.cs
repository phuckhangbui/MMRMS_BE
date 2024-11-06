using Common;
using Common.Enum;
using DTOs.MembershipRank;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Transactions;

namespace Service.Implement
{
    public class MembershipRankServiceImpl : IMembershipRankService
    {
        private readonly IMembershipRankRepository _membershipRankRepository;
        private readonly IAccountRepository _accountRepository;

        public MembershipRankServiceImpl(IMembershipRankRepository membershipRankRepository, IAccountRepository accountRepository)
        {
            _membershipRankRepository = membershipRankRepository;
            _accountRepository = accountRepository;
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

        public async Task<MembershipRankDetailDto> GetMembershipRankForCustomer(int customerId)
        {
            var membershipRank = await _membershipRankRepository.GetMembershipRankForCustomer(customerId);
            if (membershipRank == null)
            {
                throw new ServiceException(MessageConstant.MembershipRank.NoMembershipRank);
            }

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

        public async Task UpdateMembershipRankForCustomer(int customerId, double amount)
        {
            var currentMembershipRank = await _membershipRankRepository.GetMembershipRankForCustomer(customerId);
            if (currentMembershipRank == null)
            {
                throw new ServiceException(MessageConstant.MembershipRank.NoMembershipRank);
            }

            var customerAccount = await _accountRepository.GetAccounById(customerId);
            customerAccount.MoneySpent += amount;
            string paymentMadeAction = $"{GlobalConstant.MembershipRankLogPaymentMadeAction}{customerAccount.MoneySpent}";
            await _membershipRankRepository.AddMembershipRankLog(customerId, currentMembershipRank.MembershipRankId, paymentMadeAction);

            var membershipRanks = await _membershipRankRepository.GetMembershipRanks();
            MembershipRankDto? highestRankCanUpgraded = null;
            foreach (var membershipRank in membershipRanks.OrderBy(rank => rank.MoneySpent))
            {
                if (customerAccount.MoneySpent < membershipRank.MoneySpent)
                    break;

                if (customerAccount.MembershipRankId != membershipRank.MembershipRankId)
                {
                    highestRankCanUpgraded = membershipRank;
                }
            }

            if (highestRankCanUpgraded != null)
            {
                customerAccount.MembershipRankId = highestRankCanUpgraded.MembershipRankId;
                string rankUpgradedAction = $"{GlobalConstant.MembershipRankLogRankUpgradedAction}{highestRankCanUpgraded.MembershipRankName}";
                await _membershipRankRepository.AddMembershipRankLog(customerId, highestRankCanUpgraded.MembershipRankId, rankUpgradedAction);
            }

            await _accountRepository.UpdateAccount(customerAccount);
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
