using DTOs.MachineCheckRequest;

namespace Service.Interface
{
    public interface IMachineCheckRequestService
    {
        Task CancelMachineCheckRequestDetail(string machineCheckRequestId, int customerId);
        Task CreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto);
        Task<IEnumerable<MachineCheckCriteriaDto>> GetMachineCheckCriterias();
        Task CreateMachineCheckCriteria(string name);
        Task<bool> UpdateMachineCheckCriteria(int id, string name);
        Task<bool> DeleteMachineCheckCriteria(int id);
        Task<MachineCheckRequestDetailDto> GetMachineCheckRequestDetail(string machineCheckRequestId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests();
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests(int customerId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsNew();
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsOfContract(string contractId);
        Task UpdateRequestStatus(string MachineCheckRequestId, string status, int? machineTaskId);
    }
}
