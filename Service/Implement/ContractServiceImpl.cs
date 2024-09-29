using Common;
using DTOs.Contract;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class ContractServiceImpl : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IRentingRepository _rentingRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISerialNumberProductRepository _serialNumberProductRepository;

        public ContractServiceImpl(
            IContractRepository contractRepository,
            IRentingRepository rentingRepository,
            IAccountRepository accountRepository,
            ISerialNumberProductRepository serialNumberProductRepository)
        {
            _contractRepository = contractRepository;
            _rentingRepository = rentingRepository;
            _accountRepository = accountRepository;
            _serialNumberProductRepository = serialNumberProductRepository;
        }

        public async Task CreateContract(ContractRequestDto contractRequestDto)
        {
            //TODO
            //Check renting request valid
            //var isRentingRequestValid = await _rentingRepository.CheckRentingRequestValidToRent(contractRequestDto.RentingRequestId);
            //if (!isRentingRequestValid)
            //{
            //    throw new ServiceException(MessageConstant.Contract.RentingRequestInvalid);
            //}

            //Check account rent valid (Exist + Active)
            //var rentAccount = await _accountRepository.GetAccounById(contractRequestDto.AccountSignId);
            //if (rentAccount == null || !rentAccount.Status!.Equals(AccountStatusEnum.Active.ToString()))
            //{
            //    throw new ServiceException(MessageConstant.Contract.AccountRentInvalid);
            //}

            //Check list rent serail number valid (Available)
            var isSerialNumberProductsValid = await _serialNumberProductRepository.CheckSerialNumberProductsValidToRent(contractRequestDto.SerialNumberProducts);
            if (!isSerialNumberProductsValid)
            {
                throw new ServiceException(MessageConstant.Contract.SerialNumberProductsInvalid);
            }

            await _contractRepository.CreateContract(contractRequestDto);
        }

        public async Task<ContractDetailDto> GetContractDetailById(string contractId)
        {
            var contract = await CheckContractExist(contractId);

            return contract;
        }

        public async Task<IEnumerable<ContractDto>> GetContracts()
        {
            var contracts = await _contractRepository.GetContracts();

            if (contracts.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Contract.ContractListEmpty);
            }

            return contracts;
        }

        public async Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId)
        {
            var contracts = await _contractRepository.GetContractsForCustomer(customerId);

            if (contracts.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Contract.ContractListEmpty);
            }

            return contracts;
        }

        private async Task<ContractDetailDto> CheckContractExist(string contractId)
        {
            var contract = await _contractRepository.GetContractDetailById(contractId);

            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            return contract;
        }
    }
}
