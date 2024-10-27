using DTOs.Contract;
using DTOs.Invoice;

namespace Repository.Interface
{
    public interface IContractRepository
    {
        Task<IEnumerable<ContractDto>> GetContracts();
        Task<ContractDto> GetContractById(string id);
        Task<ContractDetailDto?> GetContractDetailById(string contractId);
        Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId);
        Task<string> CreateContract(int managerId, ContractRequestDto contractRequestDto);
        Task<List<ContractInvoiceDto>> SignContract(string rentingRequestId);
        Task<bool> IsContractValidToSign(string rentingRequestId);
        Task<ContractAddressDto> GetContractAddressById(string contractId);
        Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId);
        Task UpdateContractStatus(string contractId, string status);
    }
}
