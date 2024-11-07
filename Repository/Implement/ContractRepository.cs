﻿using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.Contract;
using DTOs.ContractPayment;
using DTOs.RentingRequest;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class ContractRepository : IContractRepository
    {
        private readonly IMapper _mapper;

        public ContractRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContractDto>> GetContracts(string? status)
        {
            var contracts = await ContractDao.Instance.GetContracts();

            if (!contracts.IsNullOrEmpty())
            {
                var contractDtos = _mapper.Map<IEnumerable<ContractDto>>(contracts);

                if (!string.IsNullOrEmpty(status))
                {
                    contractDtos = contractDtos.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                }

                return contractDtos;
            }

            return [];
        }

        public async Task<ContractDetailDto?> GetContractDetailById(string contractId)
        {
            var contract = await ContractDao.Instance.GetContractDetailById(contractId);
            if (contract != null)
            {
                var contractDetail = _mapper.Map<ContractDetailDto>(contract);

                var contractPayment = contractDetail.ContractPayments.FirstOrDefault(c => c.IsFirstRentalPayment == true);
                if (contractPayment != null)
                {
                    var firstRentalPayment = new FirstRentalPaymentDto()
                    {
                        DiscountPrice = contract.RentingRequest.DiscountPrice,
                        ShippingPrice = contract.RentingRequest.ShippingPrice,
                        TotalServicePrice = contract.RentingRequest.TotalServicePrice,
                    };

                    contractPayment.FirstRentalPayment = firstRentalPayment;
                }

                return contractDetail;
            }

            return null;
        }

        public async Task<IEnumerable<ContractDetailDto>> GetContractDetailListByRentingRequestId(string rentingRequestId)
        {
            var contracts = await ContractDao.Instance.GetContractsByRentingRequestId(rentingRequestId);

            if (!contracts.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<ContractDetailDto>>(contracts);
            }

            return [];
        }

        public async Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId, string? status)
        {
            var contracts = await ContractDao.Instance.GetContractsForCustomer(customerId);

            if (!contracts.IsNullOrEmpty())
            {
                var contractDtos = _mapper.Map<IEnumerable<ContractDto>>(contracts);

                if (!string.IsNullOrEmpty(status))
                {
                    contractDtos = contractDtos.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                }

                return contractDtos;
            }

            return [];
        }

        //public async Task<List<ContractInvoiceDto>> SignContract(string rentingRequestId)
        //{
        //    var (depositInvoice, rentalInvoice) = await ContractDao.Instance.SignContract(rentingRequestId);
        //    var invoiceDtos = new List<ContractInvoiceDto>();

        //    if (depositInvoice != null && rentalInvoice != null)
        //    {
        //        var depositInvoiceDto = _mapper.Map<ContractInvoiceDto>(depositInvoice);
        //        var depositContractPayments = await ContractPaymentDao.Instance.GetContractPaymentsByInvoiceId(depositInvoice.InvoiceId);
        //        if (!depositContractPayments.IsNullOrEmpty())
        //        {
        //            depositInvoiceDto.ContractPayments = _mapper.Map<List<ContractPaymentDto>>(depositContractPayments);
        //        }

        //        invoiceDtos.Add(depositInvoiceDto);

        //        //
        //        var rentalInvoiceDto = _mapper.Map<ContractInvoiceDto>(rentalInvoice);
        //        var rentalContractPayments = await ContractPaymentDao.Instance.GetContractPaymentsByInvoiceId(rentalInvoice.InvoiceId);
        //        if (!rentalContractPayments.IsNullOrEmpty())
        //        {
        //            rentalInvoiceDto.ContractPayments = _mapper.Map<List<ContractPaymentDto>>(rentalContractPayments);

        //            var firstRentalContractPayment = rentalContractPayments.FirstOrDefault(cp => (bool)cp.IsFirstRentalPayment);
        //            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(firstRentalContractPayment.Contract.RentingRequestId);

        //            var firstRentalPayment = new FirstRentalPaymentDto()
        //            {
        //                DiscountPrice = rentingRequest.DiscountPrice,
        //                ShippingPrice = rentingRequest.ShippingPrice,
        //                TotalServicePrice = rentingRequest.TotalServicePrice,
        //            };
        //            rentalInvoiceDto.ContractPayments[0].FirstRentalPayment = firstRentalPayment;
        //        }

        //        invoiceDtos.Add(rentalInvoiceDto);

        //        return invoiceDtos;
        //    }

        //    return [];
        //}



        public async Task<ContractAddressDto> GetContractAddressById(string contractId)
        {
            var address = await ContractDao.Instance.GetRentingRequestAddressByContractId(contractId);

            if (address == null)
            {
                return null;
            }

            return _mapper.Map<ContractAddressDto>(address);
        }

        public async Task<ContractDto?> GetContractById(string contractId)
        {
            var contract = await ContractDao.Instance.GetContractById(contractId);
            if (contract == null)
            {
                return null;
            }

            return _mapper.Map<ContractDto>(contract);
        }

        public async Task UpdateContractStatus(string contractId, string status)
        {
            var contract = await ContractDao.Instance.GetContractDetailById(contractId);

            contract.Status = status;

            await ContractDao.Instance.UpdateAsync(contract);
        }

        public async Task<string?> UpdateContractPayments(string invoiceId)
        {
            return await ContractPaymentDao.Instance.UpdateContractPayments(invoiceId);
        }

        public async Task<bool> IsDepositAndFirstRentalPaid(string rentingRequestId)
        {
            return await ContractPaymentDao.Instance.IsDepositAndFirstRentalPaid(rentingRequestId);
        }

        public async Task UpdateStatusContractsToSignedInRentingRequest(string rentingRequestId, DateTime paymentDate)
        {
            await ContractDao.Instance.UpdateStatusContractsToSignedInRentingRequest(rentingRequestId, paymentDate);
        }

        public async Task EndContract(string contractId, string status, int actualRentPeriod, DateTime actualDateEnd)
        {
            var contract = await ContractDao.Instance.GetContractById(contractId);
            if (contract != null)
            {
                contract.Status = status;
                contract.RentPeriod = actualRentPeriod;
                contract.DateEnd = actualDateEnd;

                var outstandingContractPayments = contract.ContractPayments
                    .Where(cp => cp.Status.Equals(ContractPaymentStatusEnum.Pending.ToString()) &&
                                !cp.Type.Equals(ContractPaymentTypeEnum.Refund.ToString()))
                    .ToList();

                if (!outstandingContractPayments.IsNullOrEmpty())
                {
                    foreach (var payment in outstandingContractPayments)
                    {
                        payment.Status = ContractPaymentStatusEnum.Canceled.ToString();
                    }
                }

                await ContractDao.Instance.UpdateAsync(contract);
            }
        }

        public async Task UpdateRefundContractPayment(string contractId, string invoiceId)
        {
            var contract = await ContractDao.Instance.GetContractById(contractId);
            if (contract != null)
            {
                var refundContractPayment = contract.ContractPayments
                    .FirstOrDefault(cp => cp.Status.Equals(ContractPaymentStatusEnum.Pending.ToString()) &&
                                        cp.Type.Equals(ContractPaymentTypeEnum.Refund.ToString()));

                if (refundContractPayment != null)
                {
                    refundContractPayment.InvoiceId = invoiceId;

                    await ContractPaymentDao.Instance.UpdateAsync(refundContractPayment);
                }
            }
        }

        public async Task<IEnumerable<ContractDto>> GetContractListOfRequest(string rentingRequestId)
        {
            var contractList = await ContractDao.Instance.GetContractsByRentingRequestId(rentingRequestId);

            return _mapper.Map<IEnumerable<ContractDto>>(contractList);
        }

        public async Task CreateContract(RentingRequestDto rentingRequestDto, RentingRequestSerialNumberDto rentingRequestSerialNumber)
        {
            var contractTerms = await TermDao.Instance.GetTermsByTermType(TermTypeEnum.Contract);

            var machineSerialNumber = await MachineSerialNumberDao.Instance.GetMachineSerialNumber(rentingRequestSerialNumber.SerialNumber);

            var contract = await InitContract(rentingRequestDto, (List<Term>)contractTerms, machineSerialNumber, rentingRequestSerialNumber.DateStart, rentingRequestSerialNumber.DateEnd);

            rentingRequestDto.TotalDepositPrice += contract.DepositPrice;
            rentingRequestDto.TotalRentPrice += contract.TotalRentPrice;
            rentingRequestDto.TotalAmount += contract.DepositPrice + contract.TotalRentPrice;

            await ContractDao.Instance.CreateAsync(contract);

            string action = $"Đã tạo hợp đồng cho số serial: {machineSerialNumber.SerialNumber} vào lúc {contract.DateCreate}";
            var log = new MachineSerialNumberLog
            {
                SerialNumber = machineSerialNumber.SerialNumber,
                Action = action,
                AccountTriggerId = rentingRequestDto.AccountOrderId,
                DateCreate = DateTime.Now,
                Type = MachineSerialNumberLogTypeEnum.Machine.ToString(),
            };
            await MachineSerialNumberLogDao.Instance.CreateAsync(log);
        }

        private async Task<Contract> InitContract(RentingRequestDto rentingRequest,
            List<Term> contractTerms,
            MachineSerialNumber machineSerialNumber,
            DateTime dateStart,
            DateTime dateEnd)
        {
            var dateCreate = DateTime.Now;
            int numberOfDays = (dateEnd - dateStart).Days + 1;

            var contract = new Contract
            {
                ContractId = await GenerateContractId(),
                SerialNumber = machineSerialNumber.SerialNumber,

                DateCreate = dateCreate,
                Status = ContractStatusEnum.NotSigned.ToString(),

                ContractName = GlobalConstant.ContractName + machineSerialNumber.SerialNumber,
                DateStart = dateStart,
                DateEnd = dateEnd,
                Content = string.Empty,
                RentingRequestId = rentingRequest.RentingRequestId,
                AccountSignId = rentingRequest.AccountOrderId,
                RentPeriod = numberOfDays,

                RentPrice = machineSerialNumber.ActualRentPrice,
                DepositPrice = machineSerialNumber.Machine!.MachinePrice * GlobalConstant.DepositValue,
                TotalRentPrice = machineSerialNumber.ActualRentPrice * numberOfDays,
            };

            //Contract Term
            foreach (var productTerm in machineSerialNumber.Machine.MachineTerms)
            {
                if (productTerm != null)
                {
                    var term = new ContractTerm()
                    {
                        Content = productTerm.Content,
                        Title = productTerm.Title,
                        DateCreate = dateCreate,
                    };

                    contract.ContractTerms.Add(term);
                }
            }

            foreach (var contractTerm in contractTerms)
            {
                if (contractTerm != null)
                {
                    var term = new ContractTerm()
                    {
                        Content = contractTerm.Content,
                        Title = contractTerm.Title,
                        DateCreate = dateCreate,
                    };

                    contract.ContractTerms.Add(term);
                }
            }

            var contractPayments = new List<ContractPayment>();
            var depositContractPayment = InitDepositContractPayment(contract);
            var refundContractPayment = InitRefundContractPayment(contract);
            contractPayments.Add(depositContractPayment);
            contractPayments.Add(refundContractPayment);

            if ((bool)rentingRequest.IsOnetimePayment)
            {
                var rentalContractPayment = new ContractPayment
                {
                    ContractId = contract.ContractId,
                    DateCreate = DateTime.Now,
                    Status = ContractPaymentStatusEnum.Pending.ToString(),
                    Type = ContractPaymentTypeEnum.Rental.ToString(),
                    Title = GlobalConstant.RentalContractPaymentTitle + contract.ContractId,
                    Amount = contract.RentPrice * contract.RentPeriod,
                    DateFrom = contract.DateStart,
                    DateTo = contract.DateEnd,
                    Period = contract.RentPeriod,
                    DueDate = contract.DateStart,
                    IsFirstRentalPayment = true,
                };

                contractPayments.Add(rentalContractPayment);
            }
            else
            {
                var currentStartDate = contract.DateStart;
                var remainingDays = numberOfDays;

                int paymentIndex = 1;
                while (remainingDays > 0)
                {
                    var paymentEndDate = GetContractPaymentEndDate((DateTime)currentStartDate, (DateTime)contract.DateEnd);

                    var paymentPeriod = (paymentEndDate - currentStartDate).Value.Days + 1;
                    var paymentAmount = contract.RentPrice * paymentPeriod;

                    var rentalContractPayment = new ContractPayment
                    {
                        ContractId = contract.ContractId,
                        DateCreate = DateTime.Now,
                        Status = ContractPaymentStatusEnum.Pending.ToString(),
                        Type = ContractPaymentTypeEnum.Rental.ToString(),
                        Title = $"{GlobalConstant.RentalContractPaymentTitle}{contract.ContractId} - Lần {paymentIndex}",
                        Amount = paymentAmount,
                        DateFrom = currentStartDate,
                        DateTo = paymentEndDate,
                        Period = paymentPeriod,
                        DueDate = currentStartDate,
                        IsFirstRentalPayment = false,
                    };

                    if (paymentIndex == 1)
                    {
                        rentalContractPayment.IsFirstRentalPayment = true;
                    }

                    contractPayments.Add(rentalContractPayment);

                    currentStartDate = paymentEndDate.AddDays(1);
                    remainingDays -= paymentPeriod;
                    paymentIndex++;
                }
            }

            contract.ContractPayments = contractPayments;

            return contract;
        }

        private async Task<string> GenerateContractId()
        {
            int currentTotalContracts = await ContractDao.Instance.GetTotalContractByDate(DateTime.UtcNow);
            string datePart = DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern);
            string sequencePart = (currentTotalContracts + 1).ToString("D4");
            return $"{GlobalConstant.ContractIdPrefixPattern}{datePart}{GlobalConstant.SequenceSeparator}{sequencePart}";
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

        private ContractPayment InitDepositContractPayment(Contract contract)
        {
            var contractPaymentDeposit = new ContractPayment
            {
                ContractId = contract.ContractId,
                DateCreate = DateTime.Now,
                Status = ContractPaymentStatusEnum.Pending.ToString(),
                Type = ContractPaymentTypeEnum.Deposit.ToString(),
                Title = GlobalConstant.DepositContractPaymentTitle + contract.ContractId,
                Amount = contract.DepositPrice,
                DateFrom = contract.DateStart,
                DateTo = contract.DateEnd,
                Period = contract.RentPeriod,
                DueDate = contract.DateStart,
                IsFirstRentalPayment = false,
            };

            return contractPaymentDeposit;
        }

        private ContractPayment InitRefundContractPayment(Contract contract)
        {
            var refundContractPayment = new ContractPayment
            {
                ContractId = contract.ContractId,
                DateCreate = DateTime.Now,
                Status = ContractPaymentStatusEnum.Pending.ToString(),
                Type = ContractPaymentTypeEnum.Refund.ToString(),
                Title = GlobalConstant.RefundContractPaymentTitle + contract.ContractId,
                Amount = contract.DepositPrice,
                DateFrom = contract.DateEnd,
                DateTo = contract.DateEnd,
                Period = contract.RentPeriod,
                DueDate = contract.DateEnd,
                IsFirstRentalPayment = false,
            };

            return refundContractPayment;
        }
    }
}
