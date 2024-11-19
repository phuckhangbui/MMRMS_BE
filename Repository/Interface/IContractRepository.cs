using DTOs.Contract;
using DTOs.ContractPayment;
using DTOs.Delivery;
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
        Task<ContractDto> EndContract(string contractId, string status, int actualRentPeriod, DateTime actualDateEnd);
        Task<IEnumerable<ContractDto>> GetContractListOfRequest(string rentingRequestId);
        Task SetInvoiceForContractPayment(string contractId, string invoiceId, string type);
        Task<ContractDto> ExtendContract(string contractId, ContractExtendDto contractExtendDto);
        Task UpdateContractDeliveryStatus(int contractDeliveryId, string status);
        Task<IEnumerable<ContractDeliveryDto>> GetContractDeliveryBaseOnContractId(string contractId);
        Task<ContractDeliveryDto> GetContractDelivery(int contractDeliveryId);
        Task<ContractPaymentDto> UpdateContractPayment(ContractPaymentDto contractPaymentDto);
        Task<ContractDto?> GetExtendContract(string baseContractId);
        Task<ContractPaymentDto?> GetContractPayment(int contractPaymentId);
    }
}
