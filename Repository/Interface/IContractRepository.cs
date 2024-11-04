using DTOs.Contract;
using DTOs.Invoice;
using DTOs.MachineSerialNumber;
using DTOs.RentingRequest;

namespace Repository.Interface
{
    public interface IContractRepository
    {
        Task<IEnumerable<ContractDto>> GetContracts();
        Task<ContractDto?> GetContractById(string contractId);
        Task<ContractDetailDto?> GetContractDetailById(string contractId);
        Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId);
        Task<(InvoiceDto DepositInvoice, InvoiceDto RentalInvoice)> CreateContract(
            RentingRequestDto rentingRequestDto,
            MachineSerialNumberDto machineSerialNumberDto,
            InvoiceDto depositInvoice,
            InvoiceDto rentalInvoice);
        Task CreateContract(RentingRequestDto rentingRequestDto, RentingRequestSerialNumberDto rentingRequestSerialNumber);
        Task<ContractAddressDto> GetContractAddressById(string contractId);
        Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId);
        Task UpdateContractStatus(string contractId, string status);
        Task<string?> UpdateContractPayments(string invoiceId);
        Task<bool> IsDepositAndFirstRentalPaid(string rentingRequestId);
        Task UpdateStatusContractsToSignedInRentingRequest(string rentingRequestId, DateTime paymentDate);
        Task ScheduleNextRentalPayment(string rentingRequestId);
        Task EndContract(string contractId, string status, int actualRentPeriod, DateTime actualDateEnd);
        Task<IEnumerable<ContractDto>> GetContractListOfRequest(string rentingRequestId);
    }
}
