﻿using Common;
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

        public ContractServiceImpl(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task CreateContract(ContractRequestDto contractRequestDto)
        {
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
