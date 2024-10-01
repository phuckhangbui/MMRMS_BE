using DTOs.MaintenanceRequest;
using Service.Interface;

namespace Service.Implement
{
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        public Task CreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsOfContract(string contractId)
        {
            throw new NotImplementedException();
        }
    }
}
