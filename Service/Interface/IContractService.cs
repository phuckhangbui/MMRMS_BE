﻿using DTOs.Contract;
using DTOs.Invoice;

namespace Service.Interface
{
    public interface IContractService
    {
        Task<IEnumerable<ContractDto>> GetContracts();
        Task<ContractDetailDto> GetContractDetailById(string contractId);
        Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId);
        Task<string> CreateContract(int managerId, ContractRequestDto contractRequestDto);
        Task<List<ContractInvoiceDto>> SignContract(string rentingRequestId);
        Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId);
    }
}
