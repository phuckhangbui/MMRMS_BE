using DTOs.MachineCheckRequest;

namespace Repository.Interface
{
    public interface IMachineCheckRequestRepository
    {
        Task CreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto);
        Task<IEnumerable<MachineCheckCriteriaDto>> GetMachineCheckCriteriaList();
        Task<MachineCheckRequestDto> GetMachineCheckRequest(string machineCheckRequestId);
        Task<MachineCheckRequestDetailDto> GetMachineCheckRequestDetail(string machineCheckRequestId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests();
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsByContractId(string contractId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsByCustomerId(int customerId);
        Task UpdateRequestStatus(string machineCheckRequestId, string status);
    }
}
