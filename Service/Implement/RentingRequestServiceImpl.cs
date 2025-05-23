﻿using Common;
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
        private readonly IBackground _background;

        public RentingRequestServiceImpl(
            IRentingRequestRepository rentingRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IAddressRepository addressRepository,
            IContractRepository contractRepository,
            IInvoiceRepository invoiceRepository,
            IBackground background)
        {
            _rentingRepository = rentingRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _addressRepository = addressRepository;
            _contractRepository = contractRepository;
            _invoiceRepository = invoiceRepository;
            _background = background;
        }

        public async Task<string> CreateRentingRequest(int customerId, NewRentingRequestDto newRentingRequestDto)
        {
            var isAddressValid = await _addressRepository.IsAddressValid(newRentingRequestDto.AddressId, customerId);
            if (!isAddressValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestAddressInvalid);
            }

            var duplicateSerials = newRentingRequestDto.RentingRequestSerialNumbers
                    .GroupBy(r => r.SerialNumber)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
            if (!duplicateSerials.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestMachinesInvalid);
            }

            var isSerialNumbersValid = await _machineSerialNumberRepository.CheckMachineSerialNumberValidToRent(newRentingRequestDto.RentingRequestSerialNumbers);
            if (!isSerialNumbersValid)
            {
                throw new ServiceException(MessageConstant.RentingRequest.RequestMachinesInvalid);
            }

            foreach (var rentingRequestSerialNumber in newRentingRequestDto.RentingRequestSerialNumbers)
            {
                if (!ValidateRentPeriod(rentingRequestSerialNumber.DateStart, rentingRequestSerialNumber.DateEnd))
                {
                    throw new ServiceException(MessageConstant.RentingRequest.RentPeriodInValid);
                }
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var rentingRequest = await _rentingRepository.CreateRentingRequest(customerId, newRentingRequestDto);

                if (rentingRequest != null)
                {
                    foreach (var rentingRequestSerialNumber in newRentingRequestDto.RentingRequestSerialNumbers)
                    {
                        await _contractRepository.CreateContract(rentingRequest, rentingRequestSerialNumber);
                    }

                    await _rentingRepository.UpdateRentingRequest(rentingRequest);
                    await _invoiceRepository.CreateInvoice(rentingRequest.RentingRequestId);
                    _background.CancelRentingRequestJob(rentingRequest.RentingRequestId);

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

        public async Task<IEnumerable<CustomerRentingRequestDto>> GetRentingRequestsForCustomer(int customerId)
        {
            return await _rentingRepository.GetRentingRequestsForCustomer(customerId);
        }

        public async Task<bool> CancelRentingRequest(string rentingRequestId)
        {
            var rentingRequest = await _rentingRepository.GetRentingRequestDetailById(rentingRequestId);
            if (rentingRequest == null || !rentingRequest.Status.Equals(RentingRequestStatusEnum.UnPaid.ToString()))
            {
                throw new ServiceException(MessageConstant.RentingRequest.RentingRequestCanNotCancel);
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var contracts = await _contractRepository.GetContractDetailListByRentingRequestId(rentingRequestId);
                if (!contracts.IsNullOrEmpty())
                {
                    foreach (var contract in contracts)
                    {
                        var isPaid = contract.ContractPayments
                            .Any(c => c.Status.Equals(ContractPaymentStatusEnum.Paid.ToString()));

                        if (isPaid)
                        {
                            throw new ServiceException(MessageConstant.RentingRequest.RentingRequestCanNotCancel);
                        }

                        await _machineSerialNumberRepository.UpdateStatus(contract.SerialNumber, MachineSerialNumberStatusEnum.Available.ToString(), (int)contract.AccountSignId, null, null);
                    }
                }

                await _rentingRepository.CancelRentingRequest(rentingRequestId);

                scope.Complete();
                return true;
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }
        }

        public async Task<IEnumerable<RentingRequestDto>> GetRentingRequestsThatStillHaveContractNeedDelivery()
        {
            var rentingRequests = await _rentingRepository.GetRentingRequests(RentingRequestStatusEnum.Signed.ToString());

            var requestsWithPendingContracts = new List<RentingRequestDto>();

            foreach (var request in rentingRequests)
            {
                var contractList = await _contractRepository.GetContractListOfRequest(request.RentingRequestId);

                bool hasNonShippingContract = contractList.Any(contract => contract.Status == ContractStatusEnum.Signed.ToString()
                                                                        || contract.Status == ContractStatusEnum.AwaitingShippingAfterCheck.ToString());

                if (hasNonShippingContract)
                {
                    requestsWithPendingContracts.Add(request);
                }
            }

            return requestsWithPendingContracts;
        }

        private bool ValidateRentPeriod(DateTime dateStart, DateTime dateEnd)
        {
            if (dateStart > DateTime.Now.AddDays(GlobalConstant.MaxStartDateOffsetInDays))
            {
                return false;
            }

            if (dateEnd < dateStart.AddDays(GlobalConstant.MinimumRentPeriodInDay))
            {
                return false;
            }

            if ((dateEnd - dateStart).Days > GlobalConstant.MaximumRentPeriodInDay)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<RentingRequestReviewResponseDto> GetRentingRequestReview(RentingRequestReviewDto rentingRequestReviewDto)
        {
            var result = new List<RentingRequestReviewResponseDto>();
            var currentStartDate = rentingRequestReviewDto.RentingRequestReviewSerialNumbers[0].DateStart;
            DateTime currentEndDate = new DateTime();

            for (int i = 0; i <= 12; i++)
            {

                var rentingRequestReviewResponseDto = new RentingRequestReviewResponseDto
                {
                    Time = i + 1,
                };

                var serialList = new List<RentingRequestReviewResponseSerialNumberDto>();
                foreach (var rentingRequestReviewSerialNumber in rentingRequestReviewDto.RentingRequestReviewSerialNumbers)
                {
                    DateTime paymentEndDate = new DateTime();

                    var rentingRequestReviewResponseSerialNumberDto = new RentingRequestReviewResponseSerialNumberDto();
                    rentingRequestReviewResponseSerialNumberDto.SerialNumber = rentingRequestReviewSerialNumber.SerialNumber;
                    rentingRequestReviewResponseSerialNumberDto.DateStart = currentStartDate;
                    rentingRequestReviewResponseSerialNumberDto.RentPrice = rentingRequestReviewSerialNumber.RentPricePerDays;

                    int numberOfDays = (rentingRequestReviewSerialNumber.DateEnd - currentStartDate).Days + 1;
                    var remainingDays = numberOfDays;

                    paymentEndDate = GetContractPaymentEndDate(currentStartDate, rentingRequestReviewResponseSerialNumberDto.DateEnd);

                    int paymentPeriod;

                    if (paymentEndDate > rentingRequestReviewSerialNumber.DateEnd)
                    {
                        paymentPeriod = (rentingRequestReviewSerialNumber.DateEnd - currentStartDate).Days + 1;
                        rentingRequestReviewResponseSerialNumberDto.DateEnd = rentingRequestReviewSerialNumber.DateEnd;
                    }
                    else
                    {
                        paymentPeriod = (paymentEndDate - currentStartDate).Days + 1;
                        rentingRequestReviewResponseSerialNumberDto.DateEnd = paymentEndDate;
                    }

                    var paymentAmount = rentingRequestReviewSerialNumber.RentPricePerDays * paymentPeriod;

                    if (paymentPeriod > 0)
                    {
                        rentingRequestReviewResponseSerialNumberDto.RentPeriod = paymentPeriod;
                        rentingRequestReviewResponseSerialNumberDto.TotalRentPrice = paymentAmount;

                        serialList.Add(rentingRequestReviewResponseSerialNumberDto);

                        currentEndDate = paymentEndDate;
                    }
                }

                currentStartDate = currentEndDate.AddDays(1);

                rentingRequestReviewResponseDto.RentingRequestReviewResponseSerialNumbers = serialList;

                if (!serialList.IsNullOrEmpty())
                {
                    result.Add(rentingRequestReviewResponseDto);
                }
            }

            return result;
        }

        private DateTime GetContractPaymentEndDate(DateTime startDate, DateTime contractEndDate)
        {
            if (startDate.Year == contractEndDate.Year && startDate.Month == contractEndDate.Month)
            {
                //DateEnd
                return contractEndDate;
            }
            else
            {
                //Full month
                return new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
            }
        }
    }
}
