using Common;
using Common.Enum;
using DTOs.MachineCheckRequest;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;

namespace Service.Implement
{
    public class MachineCheckRequestService : IMachineCheckRequestService
    {
        private readonly IMachineCheckRequestRepository _MachineCheckRequestRepository;

        private readonly IContractRepository _contractRepository;

        private readonly INotificationService _notificationService;

        private readonly IHubContext<MachineCheckRequestHub> _MachineCheckRequestHub;

        public MachineCheckRequestService(IMachineCheckRequestRepository MachineCheckRequestRepository, IContractRepository contractRepository, INotificationService notificationService, IHubContext<MachineCheckRequestHub> MachineCheckRequestHub)
        {
            _MachineCheckRequestRepository = MachineCheckRequestRepository;
            _contractRepository = contractRepository;
            _notificationService = notificationService;
            _MachineCheckRequestHub = MachineCheckRequestHub;
        }

        public async Task CreateMachineCheckRequest(int customerId, CreateMachineCheckRequestDto createMachineCheckRequestDto)
        {
            var contract = await _contractRepository.GetContractDetailById(createMachineCheckRequestDto.ContractId);

            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (DateTime.Today < contract.DateStart || DateTime.Today > contract.DateEnd)
            {
                throw new ServiceException(MessageConstant.Contract.ContractOutOfRange);
            }

            if (contract.Status != ContractStatusEnum.Shipped.ToString())
            {
                throw new ServiceException(MessageConstant.Contract.ContractIsNotReadyForRequest);
            }

            var MachineCheckRequestList = await _MachineCheckRequestRepository.GetMachineCheckRequestsByContractId(createMachineCheckRequestDto.ContractId);

            bool isFailToCreateNewRequest = MachineCheckRequestList.Any(request => request.Status == MachineCheckRequestStatusEnum.Processing.ToString() || request.Status == MachineCheckRequestStatusEnum.Assigned.ToString());

            if (isFailToCreateNewRequest)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.PendingRequestStillExist);
            }

            await _MachineCheckRequestRepository.CreateMachineCheckRequest(customerId, createMachineCheckRequestDto);

            await _notificationService.SendToManagerWhenCustomerCreateMachineCheckRequest(customerId, createMachineCheckRequestDto);

            await _MachineCheckRequestHub.Clients.All.SendAsync("OnCreateMachineCheckRequest");
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests()
        {
            return await _MachineCheckRequestRepository.GetMachineCheckRequests();
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests(int customerId)
        {
            return await _MachineCheckRequestRepository.GetMachineCheckRequestsByCustomerId(customerId);
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsOfContract(string contractId)
        {
            return await _MachineCheckRequestRepository.GetMachineCheckRequestsByContractId(contractId);
        }

        public async Task UpdateRequestStatus(string MachineCheckRequestId, string status, int accountId)
        {
            var maintenanceDto = await _MachineCheckRequestRepository.GetMachineCheckRequest(MachineCheckRequestId);

            if (maintenanceDto == null)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.RequestNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(MachineCheckRequestStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.StatusNotAvailable);
            }

            //business logic here, fix later

            await _MachineCheckRequestRepository.UpdateRequestStatus(MachineCheckRequestId, status);

            await _MachineCheckRequestHub.Clients.All.SendAsync("OnUpdateMachineCheckRequestStatus", MachineCheckRequestId);
        }
    }
}
