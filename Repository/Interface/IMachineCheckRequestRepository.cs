using DTOs.MachineCheckRequest;

namespace Repository.Interface
{
    public interface IMachineCheckRequestRepository
    {
        Task<MachineCheckRequestDto> CreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto);
        Task<IEnumerable<MachineCheckCriteriaDto>> GetMachineCheckCriteriaList();
        Task CreateMachineCheckCriteria(string name);
        Task<bool> UpdateMachineCheckCriteria(int id, string name);
        Task<bool> DeleteMachineCheckCriteria(int id);
        Task<MachineCheckRequestDto> GetMachineCheckRequest(string machineCheckRequestId);
        Task<MachineCheckRequestDetailDto> GetMachineCheckRequestDetail(string machineCheckRequestId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests();
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsByContractId(string contractId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsByCustomerId(int customerId);
        Task UpdateRequest(MachineCheckRequestDto request);
        Task UpdateRequestStatus(string machineCheckRequestId, string status);
    }
}
