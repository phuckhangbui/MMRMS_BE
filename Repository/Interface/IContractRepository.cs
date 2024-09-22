using DTOs.Contract;

namespace Repository.Interface
{
    public interface IContractRepository
    {
        Task<IEnumerable<ContractDto>> GetContracts();
        Task<ContractDto?> GetContractDetailById(string contractId);
        Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId);
        Task CreateContract(ContractRequestDto contractRequestDto);
    }
}
