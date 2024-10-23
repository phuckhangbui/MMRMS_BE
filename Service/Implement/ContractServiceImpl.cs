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
        private readonly IRentingRequestRepository _rentingRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;

        public ContractServiceImpl(
            IContractRepository contractRepository,
            IRentingRequestRepository rentingRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository)
        {
            _contractRepository = contractRepository;
            _rentingRepository = rentingRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
        }

        //TODO: Remove
        public async Task<string> CreateContract(int managerId, ContractRequestDto contractRequestDto)
        {
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
            var isMachineSerialNumbersValid = await _machineSerialNumberRepository.CheckMachineSerialNumbersValidToRent(contractRequestDto.MachineSerialNumbers);
            if (!isMachineSerialNumbersValid)
            {
                throw new ServiceException(MessageConstant.Contract.MachineSerialNumbersInvalid);
            }

            return await _contractRepository.CreateContract(managerId, contractRequestDto);
        }

        public async Task<ContractDetailDto> GetContractDetailById(string contractId)
        {
            var contract = await CheckContractExist(contractId);

            return contract;
        }

        public async Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId)
        {
            var contracts = await _contractRepository.GetContractDetailListByRentingRequestId(rentingRequestId);

            if (contracts.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Contract.ContractListEmpty);
            }

            return contracts;
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

        public async Task SignContract(string rentingRequestId)
        {
            var rentingRequest = await _rentingRepository.GetRentingRequestDetailById(rentingRequestId);
            if (rentingRequest == null)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestNotFound);
            }

            var isValidToSign = await _contractRepository.IsContractValidToSign(rentingRequestId);
            if (!isValidToSign)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotValidToSign);
            }

            await _contractRepository.SignContract(rentingRequestId);
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
