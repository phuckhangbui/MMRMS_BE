using DTOs.MaintenanceRequest;

namespace Repository.Interface
{
    public interface IMaintenanceRequestRepository
    {
        Task CreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto);
        Task<MaintenanceRequestDto> GetMaintenanceRequest(int maintenanceRequestId);
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests();
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsByContractId(string contractId);
        Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsByCustomerId(int customerId);
        Task UpdateRequestStatus(int maintenanceRequestId, string status);
    }
}
