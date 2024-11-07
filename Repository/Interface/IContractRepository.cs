using DTOs.Contract;
using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IContractRepository
    {
        Task<IEnumerable<ContractDto>> GetContracts(string? status);
        Task<ContractDto?> GetContractById(string contractId);
        Task<ContractDetailDto?> GetContractDetailById(string contractId);
        Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId, string? status);
        Task CreateContract(RentingRequestDto rentingRequestDto, RentingRequestSerialNumberDto rentingRequestSerialNumber);
        Task<ContractAddressDto> GetContractAddressById(string contractId);
        Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId);
        Task UpdateContractStatus(string contractId, string status);
        Task<string?> UpdateContractPayments(string invoiceId);
        Task<bool> IsDepositAndFirstRentalPaid(string rentingRequestId);
        Task UpdateStatusContractsToSignedInRentingRequest(string rentingRequestId, DateTime paymentDate);
        Task EndContract(string contractId, string status, int actualRentPeriod, DateTime actualDateEnd);
        Task<IEnumerable<ContractDto>> GetContractListOfRequest(string rentingRequestId);
        Task UpdateRefundContractPayment(string contractId, string invoiceId);
    }
}
