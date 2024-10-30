﻿using Common;
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
        private readonly IMachineCheckRequestRepository _machineCheckRequestRepository;

        private readonly IContractRepository _contractRepository;

        private readonly INotificationService _notificationService;

        private readonly IHubContext<MachineCheckRequestHub> _machineCheckRequestHub;

        public MachineCheckRequestService(IMachineCheckRequestRepository MachineCheckRequestRepository, IContractRepository contractRepository, INotificationService notificationService, IHubContext<MachineCheckRequestHub> MachineCheckRequestHub)
        {
            _machineCheckRequestRepository = MachineCheckRequestRepository;
            _contractRepository = contractRepository;
            _notificationService = notificationService;
            _machineCheckRequestHub = MachineCheckRequestHub;
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

            var MachineCheckRequestList = await _machineCheckRequestRepository.GetMachineCheckRequestsByContractId(createMachineCheckRequestDto.ContractId);

            bool isFailToCreateNewRequest = MachineCheckRequestList.Any(request => request.Status == MachineCheckRequestStatusEnum.New.ToString() || request.Status == MachineCheckRequestStatusEnum.Assigned.ToString());

            if (isFailToCreateNewRequest)
            {
                throw new ServiceException(MessageConstant.MachineCheckRequest.PendingRequestStillExist);
            }

            var criteriaList = await GetMachineCheckCriterias();
            var validCriteriaIds = criteriaList.Select(c => c.MachineCheckCriteriaId).ToHashSet();

            foreach (var criteria in createMachineCheckRequestDto.CheckCriterias)
            {
                if (!validCriteriaIds.Contains(criteria.MachineCheckCriteriaId))
                {
                    throw new ServiceException(MessageConstant.MachineCheckRequest.CriteriaIdNotExisted + criteria.MachineCheckCriteriaId);
                }
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

        public async Task UpdateRequestStatus(string machineCheckRequestId, string status)
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

            await _machineCheckRequestRepository.UpdateRequestStatus(machineCheckRequestId, status);

            await _machineCheckRequestHub.Clients.All.SendAsync("OnUpdateMachineCheckRequestStatus", machineCheckRequestId);
        }
    }
}
