using Common;
using Common.Enum;
using DTOs.RentingRequest;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Transactions;

namespace Service.Implement
{
    public class RentingRequestServiceImpl : IRentingRequestService
    {
        private readonly IRentingRequestRepository _rentingRepository;
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public RentingRequestServiceImpl(
            IRentingRequestRepository rentingRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IAddressRepository addressRepository,
            IContractRepository contractRepository,
            IInvoiceRepository invoiceRepository)
        {
            _rentingRepository = rentingRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _addressRepository = addressRepository;
            _contractRepository = contractRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto)
        {
            var duplicateSerials = newRentingRequestDto.RentingRequestSerialNumbers
                    .GroupBy(r => r.SerialNumber)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
            if (!duplicateSerials.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestMachinesInvalid);
            }

            var isAddressValid = await _addressRepository.IsAddressValid(newRentingRequestDto.AddressId, customerId);
            if (!isAddressValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestAddressInvalid);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var rentingRequest = await _rentingRepository.CreateRentingRequest(customerId, newRentingRequestDto);

                    if (rentingRequest != null)
                    {
                        var isSerialNumbersValid = await _machineSerialNumberRepository.CheckMachineSerialNumberValidToRent(newRentingRequestDto.RentingRequestSerialNumbers);
                        if (!isSerialNumbersValid)
                        {
                            throw new ServiceException(MessageConstant.RentingRequest.RequestMachinesInvalid);
                        }

                        foreach (var rentingRequestSerialNumber in newRentingRequestDto.RentingRequestSerialNumbers)
                        {
                            await _contractRepository.CreateContract(rentingRequest, rentingRequestSerialNumber);
                        }

                        await _rentingRepository.UpdateRentingRequest(rentingRequest);
                        await _invoiceRepository.CreateInvoice(rentingRequest.RentingRequestId);
                        _rentingRepository.ScheduleCancelRentingRequest(rentingRequest.RentingRequestId);

                        scope.Complete();

                        return rentingRequest.RentingRequestId;
                    }
                    else
                    {
                        throw new ServiceException(MessageConstant.RentingRequest.CreateRentingRequestFail);
                    }
                }
                catch (Exception ex)
                {
                    throw new ServiceException(ex.Message);
                }
            }
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequests(string? status)
        {
            if (!string.IsNullOrEmpty(status) && !Enum.IsDefined(typeof(RentingRequestStatusEnum), status))
            {
                throw new ServiceException(MessageConstant.InvalidStatusValue);
            }

            return await _rentingRepository.GetRentingRequests(status);
        }

        public async Task<RentingRequestDetailDto> GetRentingRequestDetail(string rentingRequestId)
        {
            var rentingRequest = await _rentingRepository.GetRentingRequestDetailById(rentingRequestId);
            if (rentingRequest == null)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestNotFound);
            }

            return rentingRequest;
        }

        public async Task<RentingRequestInitDataDto> InitializeRentingRequestData(int customerId, RentingRequestMachineInRangeDto rentingRequestMachineInRangeDto)
        {
            return await _rentingRepository.InitializeRentingRequestData(customerId, rentingRequestMachineInRangeDto);
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequestsForCustomer(int customerId)
        {
            return await _rentingRepository.GetRentingRequestsForCustomer(customerId);
        }

        public async Task<bool> CancelRentingRequest(string rentingRequestId)
        {
            var isValid = await _rentingRepository.IsRentingRequestValidToCancel(rentingRequestId);
            if (!isValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestCanNotCancel);
            }

            return await _rentingRepository.CancelRentingRequest(rentingRequestId);
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequestsThatStillHaveContractNeedDelivery()
        {
            var rentingRequests = await _rentingRepository.GetRentingRequests(RentingRequestStatusEnum.Signed.ToString());

            var requestsWithPendingContracts = new List<RentingRequestDto>();

            foreach (var request in rentingRequests)
            {
                var contractList = await _contractRepository.GetContractListOfRequest(request.RentingRequestId);

                bool hasNonShippingContract = contractList.Any(contract => contract.Status != ContractStatusEnum.Shipping.ToString());

                if (hasNonShippingContract)
                {
                    requestsWithPendingContracts.Add(request);
                }
            }

            return requestsWithPendingContracts;
        }

    }
}
