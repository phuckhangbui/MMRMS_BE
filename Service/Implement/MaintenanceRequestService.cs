using Common;
using Common.Enum;
using DTOs.MaintenanceRequest;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;

namespace Service.Implement
{
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        private readonly IMaintenanceRequestRepository _maintenanceRequestRepository;

        private readonly IContractRepository _contractRepository;

        private readonly INotificationService _notificationService;

        private readonly IHubContext<MaintenanceRequestHub> _maintenanceRequestHub;

        public MaintenanceRequestService(IMaintenanceRequestRepository maintenanceRequestRepository, IContractRepository contractRepository, INotificationService notificationService, IHubContext<MaintenanceRequestHub> maintenanceRequestHub)
        {
            _maintenanceRequestRepository = maintenanceRequestRepository;
            _contractRepository = contractRepository;
            _notificationService = notificationService;
            _maintenanceRequestHub = maintenanceRequestHub;
        }

        public async Task CreateMaintenanceRequest(int customerId, CreateMaintenanceRequestDto createMaintenanceRequestDto)
        {
            var contract = await _contractRepository.GetContractDetailById(createMaintenanceRequestDto.ContractId);

            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (DateTime.Today < contract.DateStart || DateTime.Today > contract.DateEnd)
            {
                throw new ServiceException(MessageConstant.Contract.ContractOutOfRange);
            }

            await _maintenanceRequestRepository.CreateMaintenanceRequest(customerId, createMaintenanceRequestDto);

            await _notificationService.SendToManagerWhenCustomerCreateMaintenanceRequest(customerId, createMaintenanceRequestDto);

            await _maintenanceRequestHub.Clients.All.SendAsync("OnUpdateMaintenanceRequest");
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

        public async Task UpdateRequestStatus(int maintenanceRequestId, string status, int accountId)
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


            await _maintenanceRequestHub.Clients.All.SendAsync("OnUpdateMaintenanceRequest");
        }
    }
}
