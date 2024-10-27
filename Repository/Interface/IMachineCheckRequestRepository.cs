using DTOs.MachineCheckRequest;

namespace Repository.Interface
{
    public interface IMachineCheckRequestRepository
    {
        Task CreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto);
        Task<MachineCheckRequestDto> GetMachineCheckRequest(string MachineCheckRequestId);
        Task<MachineCheckRequestDetailDto> GetMachineCheckRequestDetail(string machineCheckRequestId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests();
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsByContractId(string contractId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsByCustomerId(int customerId);
        Task UpdateRequestStatus(string MachineCheckRequestId, string status);
    }
}
