using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.ComponentReplacementTicket;
using DTOs.Contract;
using DTOs.ContractPayment;
using DTOs.Delivery;
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

                var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(contract.RentingRequestId);
                if (rentingRequest != null)
                {
                    var numOfContracts = rentingRequest.Contracts.Count;

                    contractDetail.ShippingDistance = rentingRequest.ShippingDistance ?? 0;
                    contractDetail.ProvisionalShippingPrice = rentingRequest.ShippingPrice / numOfContracts ?? 0;
                    contractDetail.ServicePrice = rentingRequest.TotalServicePrice / numOfContracts ?? 0;
                }

                var componentReplacementTickets = await ComponentReplacementTicketDao.Instance.GetComponentReplacementTicketByContractId(contractId);
                if (!componentReplacementTickets.IsNullOrEmpty())
                {
                    contractDetail.ComponentReplacementTickets = _mapper.Map<List<ComponentReplacementTicketDto>>(componentReplacementTickets);
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
            if (status.Equals(ContractStatusEnum.Signed.ToString()))
            {
                contract.DateSign = DateTime.Now;
            }

            await ContractDao.Instance.UpdateAsync(contract);
        }

        public async Task UpdateContractStatusToRenting(string contractId, double deliveryRefund)
        {
            var contract = await ContractDao.Instance.GetContractDetailById(contractId);

            var oldDeposit = contract.DepositPrice;

            contract.Status = ContractStatusEnum.Renting.ToString();
            if (deliveryRefund > 0)
            {
                contract.RefundShippingPrice = deliveryRefund;
                contract.DepositPrice = oldDeposit + deliveryRefund;
                contract.Content = $"Hoàn trả tiền vận chuyển tạm tính là: {NumberExtension.FormatToVND(deliveryRefund)} " +
                                    $"vào tiền đặt cọc ban đầu: {NumberExtension.FormatToVND(oldDeposit)}, " +
                                    $"tiền đặt cọc mới là {NumberExtension.FormatToVND(contract.DepositPrice)} sẽ được hoàn trả khi tất toán hợp đồng";
            }

            await ContractDao.Instance.UpdateAsync(contract);
        }

        public async Task<ContractDto> EndContract(string contractId, string status, int actualRentPeriod, DateTime actualDateEnd)
        {
            var contract = await ContractDao.Instance.GetContractById(contractId);
            if (contract != null)
            {
                contract.Status = status;
                contract.RentPeriod = actualRentPeriod;
                contract.DateEnd = actualDateEnd;

                var outstandingContractPayments = contract.ContractPayments
                    .Where(cp => cp.Status.Equals(ContractPaymentStatusEnum.Pending.ToString()) &&
                                (cp.Type.Equals(ContractPaymentTypeEnum.Rental.ToString()) ||
                                cp.Type.Equals(ContractPaymentTypeEnum.Extend.ToString())))
                    .ToList();

                if (!outstandingContractPayments.IsNullOrEmpty())
                {
                    foreach (var payment in outstandingContractPayments)
                    {
                        payment.Status = ContractPaymentStatusEnum.Canceled.ToString();
                        await ContractPaymentDao.Instance.UpdateAsync(payment);

                        var invoiceId = payment.InvoiceId;
                        var invoice = await InvoiceDao.Instance.GetInvoice(invoiceId);
                        if (invoice != null)
                        {
                            invoice.Amount -= payment.Amount;
                            if (invoice.Amount <= 0)
                            {
                                invoice.Amount = 0;
                                invoice.Status = InvoiceStatusEnum.Canceled.ToString();
                            }

                            await InvoiceDao.Instance.UpdateAsync(invoice);
                        }
                    }
                }

                contract = await ContractDao.Instance.UpdateAsync(contract);
            }

            return _mapper.Map<ContractDto>(contract);
        }

        public async Task SetInvoiceForContractPayment(string contractId, string invoiceId, string type)
        {
            var contract = await ContractDao.Instance.GetContractById(contractId);
            if (contract != null)
            {
                var contractPayment = contract.ContractPayments
                    .FirstOrDefault(cp => cp.Status.Equals(ContractPaymentStatusEnum.Pending.ToString()) &&
                                        cp.Type.Equals(type));

                if (contractPayment != null)
                {
                    contractPayment.InvoiceId = invoiceId;

                    if (type.Equals(ContractPaymentTypeEnum.Refund.ToString()) || type.Equals(ContractPaymentTypeEnum.Fine.ToString()))
                    {
                        contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();
                        contractPayment.CustomerPaidDate = DateTime.Now;
                    }

                    await ContractPaymentDao.Instance.UpdateAsync(contractPayment);
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

            var oldStatus = machineSerialNumber.Status;
            machineSerialNumber.Status = MachineSerialNumberStatusEnum.Reserved.ToString();
            await MachineSerialNumberDao.Instance.UpdateAsync(machineSerialNumber);

            var machineSerialNumberLog = new MachineSerialNumberLog
            {
                SerialNumber = machineSerialNumber.SerialNumber,
                AccountTriggerId = rentingRequestDto.AccountOrderId,
                DateCreate = DateTime.Now,
                Type = MachineSerialNumberLogTypeEnum.Machine.ToString(),
                Action = $"Thay đổi trạng thái từ [{EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(oldStatus)}] thành [{EnumExtensions.TranslateStatus<MachineSerialNumberStatusEnum>(machineSerialNumber.Status)}]",
            };

            await MachineSerialNumberLogDao.Instance.CreateAsync(machineSerialNumberLog);
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
                IsExtended = false,

                RentPrice = machineSerialNumber.ActualRentPrice,
                DepositPrice = machineSerialNumber.Machine!.MachinePrice * GlobalConstant.DepositValue,
                TotalRentPrice = machineSerialNumber.ActualRentPrice * numberOfDays,

                RefundShippingPrice = 0,
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
                    else
                    {
                        rentalContractPayment.DueDate = rentalContractPayment.DueDate.Value.AddDays(GlobalConstant.DueDateContractPayment);
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

        public async Task<ContractDto> ExtendContract(string contractId, ContractExtendDto contractExtendDto)
        {
            var baseContract = await ContractDao.Instance.GetContractById(contractId);

            var dateCreate = DateTime.Now;
            int numberOfDays = (contractExtendDto.DateEnd - baseContract.DateEnd.Value).Days + 1;

            var contract = new Contract
            {
                ContractId = await GenerateContractId(),
                SerialNumber = baseContract.SerialNumber,

                DateCreate = dateCreate,
                Status = ContractStatusEnum.NotSigned.ToString(),

                ContractName = GlobalConstant.ContractName + baseContract.SerialNumber,
                DateStart = baseContract.DateEnd,
                DateEnd = contractExtendDto.DateEnd,
                Content = string.Empty,
                AccountSignId = baseContract.AccountSignId,
                RentingRequestId = baseContract.RentingRequestId,
                RentPeriod = numberOfDays,
                RentPrice = baseContract.RentPrice,
                DepositPrice = 0,
                TotalRentPrice = baseContract.RentPrice * numberOfDays,
                BaseContractId = contractId,
                IsExtended = false,
            };

            var extendContractPayment = new ContractPayment
            {
                ContractId = contract.ContractId,
                DateCreate = DateTime.Now,
                Status = ContractPaymentStatusEnum.Pending.ToString(),
                Type = ContractPaymentTypeEnum.Extend.ToString(),
                Title = GlobalConstant.ExtendContractPaymentTitle + contract.ContractId,
                Amount = contract.RentPrice * contract.RentPeriod,
                DateFrom = contract.DateStart,
                DateTo = contract.DateEnd,
                Period = contract.RentPeriod,
                DueDate = contract.DateStart,
                IsFirstRentalPayment = false,
            };

            contract.ContractPayments.Add(extendContractPayment);

            contract = await ContractDao.Instance.CreateAsync(contract);

            baseContract.IsExtended = true;
            await ContractDao.Instance.UpdateAsync(baseContract);

            return _mapper.Map<ContractDto>(contract);
        }

        private async Task<string> GenerateContractId()
        {
            int currentTotalContracts = await ContractDao.Instance.GetLastestContractByDate(DateTime.Now);
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

        public async Task<ContractDeliveryDto> GetContractDelivery(int contractDeliveryId)
        {
            var contractDelivery = await ContractDeliveryDao.Instance.GetContractDeliveryById(contractDeliveryId);

            return _mapper.Map<ContractDeliveryDto>(contractDelivery);
        }

        public async Task<IEnumerable<ContractDeliveryDto>> GetContractDeliveryBaseOnContractId(string contractId)
        {
            var list = await ContractDeliveryDao.Instance.GetContractDeliveryBaseOnContractId(contractId);

            return _mapper.Map<IEnumerable<ContractDeliveryDto>>(list);
        }

        public async Task UpdateContractDeliveryStatus(int contractDeliveryId, string status)
        {
            var contractDelivery = await ContractDeliveryDao.Instance.GetContractDeliveryById(contractDeliveryId);

            contractDelivery.Status = status;

            await ContractDeliveryDao.Instance.UpdateAsync(contractDelivery);
        }

        public async Task<ContractPaymentDto> UpdateContractPayment(ContractPaymentDto contractPaymentDto)
        {
            var contractPayment = _mapper.Map<ContractPayment>(contractPaymentDto);

            contractPayment = await ContractPaymentDao.Instance.UpdateAsync(contractPayment);

            return _mapper.Map<ContractPaymentDto>(contractPayment);
        }

        public async Task<ContractDto?> GetExtendContract(string baseContractId)
        {
            var extendContract = await ContractDao.Instance.GetExtendContract(baseContractId);
            if (extendContract == null)
            {
                return null;
            }

            return _mapper.Map<ContractDto>(extendContract);
        }

        public async Task<ContractPaymentDto?> GetContractPayment(int contractPaymentId)
        {
            var contractPayment = await ContractPaymentDao.Instance.GetContractPaymentById(contractPaymentId);
            if (contractPayment == null)
            {
                return null;
            }

            return _mapper.Map<ContractPaymentDto>(contractPayment);
        }

        public async Task<IEnumerable<ContractDto>> GetRentalHistoryOfSerialNumber(string serialNumber)
        {
            var contracts = await ContractDao.Instance.GetRentalHistoryOfSerialNumber(serialNumber);

            if (!contracts.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<ContractDto>>(contracts);
            }

            return [];
        }

        public async Task<ContractPaymentDto> CreateFineContractPayment(string contractId)
        {
            var fineContractPayment = new ContractPayment
            {
                ContractId = contractId,
                DateCreate = DateTime.Now,
                Status = ContractPaymentStatusEnum.Pending.ToString(),
                Type = ContractPaymentTypeEnum.Fine.ToString(),
                Title = GlobalConstant.FineContractPaymentTitle + contractId,
                Amount = GlobalConstant.FineValue,
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
                Period = 1,
                DueDate = DateTime.Now,
                IsFirstRentalPayment = false,
            };

            fineContractPayment = await ContractPaymentDao.Instance.CreateAsync(fineContractPayment);

            return _mapper.Map<ContractPaymentDto>(fineContractPayment);
        }
    }
}
