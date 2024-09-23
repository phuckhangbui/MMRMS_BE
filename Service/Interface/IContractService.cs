﻿using DTOs.Contract;

namespace Service.Interface
{
    public interface IContractService
    {
        Task<IEnumerable<ContractDto>> GetContracts();
        Task<ContractDetailDto> GetContractDetailById(string contractId);
        Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId);
        Task CreateContract(ContractRequestDto contractRequestDto);
    }
}
