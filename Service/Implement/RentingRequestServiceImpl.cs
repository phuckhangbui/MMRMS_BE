using Common;
using Common.Enum;
using DTOs.RentingRequest;
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
            newRentingRequestDto.RentingRequestMachineDetails = newRentingRequestDto.RentingRequestMachineDetails
                .GroupBy(m => m.MachineId)
                .Select(g => new NewRentingRequestMachineDetailDto
                {
                    MachineId = g.Key,
                    Quantity = g.Sum(m => m.Quantity)
                })
                .ToList();

            //Check address valid
            var isAddressValid = await _addressRepository.IsAddressValid(newRentingRequestDto.AddressId, customerId);
            if (!isAddressValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestAddressInvalid);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Create renting request
                    var rentingRequest = await _rentingRepository.CreateRentingRequest(customerId, newRentingRequestDto);

                    if (rentingRequest != null)
                    {
                        var machineIds = newRentingRequestDto.RentingRequestMachineDetails.Select(m => m.MachineId).Distinct().ToList();
                        var allAvailableSerialNumbers = await _machineSerialNumberRepository.GetMachineSerialNumberAvailablesToRent(machineIds, newRentingRequestDto.DateStart, newRentingRequestDto.DateEnd);

                        var (depositInvoice, rentalInvoice) = await _invoiceRepository.InitInvoices(rentingRequest);

                        //Loop create contract
                        foreach (var newRentingRequestMachine in newRentingRequestDto.RentingRequestMachineDetails)
                        {
                            //var availaleSerialNumbers = await _machineSerialNumberRepository.GetMachineSerialNumberAvailablesToRent(newRentingRequestMachine.MachineId, newRentingRequestDto.DateStart, newRentingRequestDto.DateEnd);
                            //var selectedSerialNumbers = availaleSerialNumbers
                            //    .Take(newRentingRequestMachine.Quantity)
                            //    .ToList();

                            var selectedSerialNumbers = allAvailableSerialNumbers
                                .Where(s => s.MachineId == newRentingRequestMachine.MachineId)
                                .Take(newRentingRequestMachine.Quantity)
                                .ToList();

                            if (selectedSerialNumbers.Count < newRentingRequestMachine.Quantity)
                            {
                                throw new ServiceException(MessageConstant.RentingRequest.RequestMachinesInvalid);
                            }

                            foreach (var machineSerialNumber in selectedSerialNumbers)
                            {
                                (depositInvoice, rentalInvoice) = await _contractRepository.CreateContract(rentingRequest, machineSerialNumber, depositInvoice, rentalInvoice);
                            }
                        }

                        depositInvoice.Amount = rentingRequest.TotalDepositPrice;
                        if (rentingRequest.IsOnetimePayment == true)
                        {
                            rentalInvoice.Amount = rentingRequest.TotalRentPrice + rentingRequest.TotalServicePrice + rentingRequest.ShippingPrice
                                - rentingRequest.DiscountPrice;
                        }
                        else
                        {
                            rentalInvoice.Amount += rentingRequest.ShippingPrice - rentingRequest.DiscountPrice;
                        }

                        //await _rentingRepository.UpdateRentingRequest(rentingRequest);
                        await _invoiceRepository.UpdateInvoice(depositInvoice);
                        await _invoiceRepository.UpdateInvoice(rentalInvoice);

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
            var rentingRequests = await _rentingRepository.GetRentingRequests();

            if (!string.IsNullOrEmpty(status))
            {
                if (!Enum.IsDefined(typeof(RentingRequestStatusEnum), status))
                {
                    throw new ServiceException(MessageConstant.InvalidStatusValue);
                }

                rentingRequests = rentingRequests.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            return rentingRequests;
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
    }
}
