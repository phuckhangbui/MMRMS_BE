using DTOs.Contract;

namespace Service.Interface
{
    public interface IContractService
    {
        Task<IEnumerable<ContractDto>> GetContracts(string? status);
        Task<IEnumerable<ContractDto>> GetRentalHistoryOfSerialNumber(string serialNumber);
        Task<ContractDetailDto> GetContractDetail(string contractId, int accountId);
        Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId, string? status);
        Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId);
        Task<bool> EndContract(int accountId, string contractId);
        Task<bool> ExtendContract(int accountId, string contractId, ContractExtendDto contractExtendDto);
        Task CancelExtendContract(string extendContractId);
    }
}
