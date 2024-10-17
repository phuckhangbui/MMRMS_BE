using DTOs.MaintenanceRequest;

namespace Service.Interface
{
    public interface IMaintenanceRequestService
    {
        Task CreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto);
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests();
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests(int customerId);
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsOfContract(string contractId);
        Task UpdateRequestStatus(string maintenanceRequestId, string status, int accountId);
    }
}
