using Common;
using Common.Enum;
using DTOs.MachineCheckRequest;
using DTOs.MachineTask;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;
using System.Transactions;

namespace Service.Implement
{
    public class MachineCheckRequestService : IMachineCheckRequestService
    {
        private readonly IMachineCheckRequestRepository _machineCheckRequestRepository;

        private readonly IContractRepository _contractRepository;

        private readonly INotificationService _notificationService;

        private readonly IMachineTaskRepository _machineTaskRepository;

        private readonly IHubContext<MachineCheckRequestHub> _machineCheckRequestHub;

        private readonly IHubContext<MachineTaskHub> _machineTaskHub;

        public MachineCheckRequestService(IMachineCheckRequestRepository MachineCheckRequestRepository, IContractRepository contractRepository, INotificationService notificationService, IHubContext<MachineCheckRequestHub> MachineCheckRequestHub, IMachineTaskRepository machineTaskRepository, IHubContext<MachineTaskHub> machineTaskHub)
        {
            _machineCheckRequestRepository = MachineCheckRequestRepository;
            _contractRepository = contractRepository;
            _notificationService = notificationService;
            _machineCheckRequestHub = MachineCheckRequestHub;
            _machineTaskRepository = machineTaskRepository;
            _machineTaskHub = machineTaskHub;
        }

        public async Task CancelMachineCheckRequestDetail(string machineCheckRequestId, int customerId)
        {
            var request = await _machineCheckRequestRepository.GetMachineCheckRequest(machineCheckRequestId);

            if (request == null)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.RequestNotFound);
            }

            var contract = await _contractRepository.GetContractById(request.ContractId);

            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (contract.AccountSignId != customerId)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.NotCorrectCustomer);
            }

            if (request.Status != MachineCheckRequestStatusEnum.New.ToString() &&
               request.Status != MachineCheckRequestStatusEnum.Assigned.ToString())
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.RequestCannotCancel);
            }

            MachineTaskDto machineTask = null;

            if (request.MachineTaskId != null)
            {
                machineTask = await _machineTaskRepository.GetMachineTask((int)request.MachineTaskId);

                if (machineTask != null && machineTask?.DateStart <= DateTime.Now)
                {
                    throw new ServiceException(MessageConstant.MachineCheckRequest.RequestCannotCancel);
                }
            }


            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await this.UpdateRequestStatus(request.MachineCheckRequestId, MachineCheckRequestStatusEnum.Canceled.ToString(), null);

                    await _machineTaskRepository.UpdateTaskStatus((int)request.MachineTaskId, MachineTaskStatusEnum.Canceled.ToString(), customerId, null);

                    if (machineTask != null)
                    {
                        await _notificationService.SendNotificationToStaffWhenTaskStatusUpdated((int)machineTask.StaffId, machineTask.MachineTaskId, MachineTaskStatusEnum.Canceled.ToVietnamese());
                    }
                    await _machineCheckRequestHub.Clients.All.SendAsync("OnUpdateMachineTask", (int)request.MachineTaskId);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.MachineCheckRequest.CancelRequestFail);
                }
            }
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

            if (contract.Status != ContractStatusEnum.Renting.ToString())
            {
                throw new ServiceException(MessageConstant.Contract.ContractIsNotReadyForRequest);
            }

            var machineCheckRequestList = await _machineCheckRequestRepository.GetMachineCheckRequestsByContractId(createMachineCheckRequestDto.ContractId);

            bool isFailToCreateNewRequest = machineCheckRequestList.Any(request => request.Status == MachineCheckRequestStatusEnum.New.ToString() || request.Status == MachineCheckRequestStatusEnum.Assigned.ToString());

            if (isFailToCreateNewRequest)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.PendingRequestStillExist);
            }

            await _machineCheckRequestRepository.CreateMachineCheckRequest(customerId, createMachineCheckRequestDto);

            await _notificationService.SendToManagerWhenCustomerCreateMachineCheckRequest(customerId, createMachineCheckRequestDto);

            await _machineCheckRequestHub.Clients.All.SendAsync("OnCreateMachineCheckRequest");
        }

        public async Task<IEnumerable<MachineCheckCriteriaDto>> GetMachineCheckCriterias()
        {
            return await _machineCheckRequestRepository.GetMachineCheckCriteriaList();
        }

        public async Task<MachineCheckRequestDetailDto> GetMachineCheckRequestDetail(string machineCheckRequestId)
        {
            var requestDetailDto = await _machineCheckRequestRepository.GetMachineCheckRequestDetail(machineCheckRequestId);

            if (requestDetailDto == null)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.RequestNotFound);
            }

            return requestDetailDto;
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests()
        {
            return await _machineCheckRequestRepository.GetMachineCheckRequests();
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequests(int customerId)
        {
            return await _machineCheckRequestRepository.GetMachineCheckRequestsByCustomerId(customerId);
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsNew()
        {
            var list = await _machineCheckRequestRepository.GetMachineCheckRequests();

            return list.Where(r => r.Status == MachineCheckRequestStatusEnum.New.ToString()).ToList();
        }

        public async Task<IEnumerable<MachineCheckRequestDto>> GetMachineCheckRequestsOfContract(string contractId)
        {
            return await _machineCheckRequestRepository.GetMachineCheckRequestsByContractId(contractId);
        }

        public async Task UpdateRequestStatus(string machineCheckRequestId, string status, int? machineTaskId)
        {
            var request = await _machineCheckRequestRepository.GetMachineCheckRequest(machineCheckRequestId);

            if (request == null)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.RequestNotFound);
            }

            if (string.IsNullOrEmpty(status) || !Enum.TryParse(typeof(MachineCheckRequestStatusEnum), status, true, out _))
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.StatusNotAvailable);
            }

            request.Status = status;

            var contract = await _contractRepository.GetContractById(request.ContractId);
            if (contract == null)
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotFound);
            }

            if (machineTaskId != null && status == MachineCheckRequestStatusEnum.Assigned.ToString())
            {
                request.MachineTaskId = machineTaskId;
            }

            await _machineCheckRequestRepository.UpdateRequest(request);

            //send notification here
            await _notificationService.SendNotificationToCustomerWhenUpdateRequestStatus((int)contract.AccountSignId, request);

            await _machineCheckRequestHub.Clients.All.SendAsync("OnUpdateMachineCheckRequest", machineCheckRequestId);
        }
    }
}
