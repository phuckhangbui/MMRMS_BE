using DTOs.Contract;

namespace Repository.Interface
{
    public interface IContractRepository
    {
        Task<IEnumerable<ContractDto>> GetContracts();
        Task<ContractDetailDto?> GetContractDetailById(string contractId);
        Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId);
        Task<string> CreateContract(int managerId, ContractRequestDto contractRequestDto);
        Task SignContract(string rentingRequestId);

        Task<ContractAddressDto> GetContractAddressById(string contractId);
        Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId);
    }
}
