using DTOs.MachineCheckRequest;

namespace Service.Interface
{
    public interface IMachineCheckRequestService
    {
        Task CreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto);
        Task<IEnumerable<MachineCheckCriteriaDto>> GetMachineCheckCriterias();
        Task<MachineCheckRequestDetailDto> GetMachineCheckRequestDetail(string machineCheckRequestId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests();
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests(int customerId);
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsNew();
        Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsOfContract(string contractId);
        Task UpdateRequestStatus(string MachineCheckRequestId, string status, int? machineTaskId);
    }
}
