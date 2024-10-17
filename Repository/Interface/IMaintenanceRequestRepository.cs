using DTOs.MaintenanceRequest;

namespace Repository.Interface
{
    public interface IMaintenanceRequestRepository
    {
        Task CreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto);
        Task<MaintenanceRequestDto> GetMaintenanceRequest(string maintenanceRequestId);
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests();
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsByContractId(string contractId);
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsByCustomerId(int customerId);
        Task UpdateRequestStatus(string maintenanceRequestId, string status);
    }
}
