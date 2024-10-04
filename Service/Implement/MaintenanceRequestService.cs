using Common;
using Common.Enum;
using DTOs.MaintenanceRequest;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        private readonly IMaintenanceRequestRepository _maintenanceRequestRepository;

        public MaintenanceRequestService(IMaintenanceRequestRepository maintenanceRequestRepository)
        {
            _maintenanceRequestRepository = maintenanceRequestRepository;
        }

        public async Task CreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto)
        {
            await _maintenanceRequestRepository.CreateMaintenanceRequest(customerId, createMaintenanceRequestDto);
        }

        public async Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests()
        {
            return await _maintenanceRequestRepository.GetMaintenanceRequests();
        }

        public async Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequests(int customerId)
        {
            return await _maintenanceRequestRepository.GetMaintenanceRequestsByCustomerId(customerId);
        }

        public async Task<IEnumerable<MaintenanceRequestDto>> GetMaintenanceRequestsOfContract(string contractId)
        {
            return await _maintenanceRequestRepository.GetMaintenanceRequestsByContractId(contractId);
        }

        public async Task UpdateRequestStatus(int maintenanceRequestId, string status)
        {
            var maintenanceDto = await _maintenanceRequestRepository.GetMaintenanceRequest(maintenanceRequestId);

            if (maintenanceDto == null)
            {
                throw new ServiceException(MessageConstant.MaintenanceRequest.RequestNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(MaintenanceRequestStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.MaintenanceRequest.StatusNotAvailable);
            }

            //business logic here, fix later

            await _maintenanceRequestRepository.UpdateRequestStatus(maintenanceRequestId, status);
        }
    }
}
