using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.Contract;
using DTOs.ContractPayment;
using DTOs.Invoice;
using DTOs.MachineSerialNumber;
using DTOs.RentingRequest;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<ContractDto>> GetContracts()
        {
            var contracts = await ContractDao.Instance.GetContracts();

            if (!contracts.IsNullOrEmpty())
            {
                var contractDtos = _mapper.Map<IEnumerable<ContractDto>>(contracts);
                return contractDtos;
            }

            return [];
        }

        public async Task<ContractDetailDto?> GetContractDetailById(string contractId)
        {
            var contract = await ContractDao.Instance.GetContractById(contractId);
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

        public async Task<IEnumerable<ContractDto>> GetContractsForCustomer(int customerId)
        {
            var contracts = await ContractDao.Instance.GetContractsForCustomer(customerId);

            if (!contracts.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<ContractDto>>(contracts);
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

        public async Task<(InvoiceDto DepositInvoice, InvoiceDto RentalInvoice)> CreateContract(RentingRequestDto rentingRequestDto,
            MachineSerialNumberDto machineSerialNumberDto,
            InvoiceDto depositInvoice,
            InvoiceDto rentalInvoice)
        {
            using var context = new MmrmsContext();

            var contractTerms = await context.Terms
                .Where(t => t.Type.Equals(TermTypeEnum.Contract.ToString()))
                .ToListAsync();

            var machineSerialNumber = await context.MachineSerialNumbers
                .Include(m => m.Machine)
                    .ThenInclude(m => m.MachineTerms)
                .FirstOrDefaultAsync(m => m.SerialNumber.Equals(machineSerialNumberDto.SerialNumber));

            var rentingRequest = await context.RentingRequests
                .Include(r => r.ServiceRentingRequests)
                .FirstOrDefaultAsync(r => r.RentingRequestId.Equals(rentingRequestDto.RentingRequestId));
            double totalServicePricePerContract = (double)rentingRequest.ServiceRentingRequests.Select(s => s.ServicePrice).Sum();

            var contract = InitContract(rentingRequestDto, contractTerms, machineSerialNumber, totalServicePricePerContract, depositInvoice, rentalInvoice);

            rentingRequestDto.TotalDepositPrice += contract.DepositPrice;
            rentingRequestDto.TotalRentPrice += contract.TotalRentPrice;
            rentingRequestDto.TotalServicePrice += totalServicePricePerContract;
            rentingRequestDto.TotalAmount += contract.DepositPrice + contract.TotalRentPrice + totalServicePricePerContract;

            rentingRequest.Contracts.Add(contract);
            await context.SaveChangesAsync();

            return (depositInvoice, rentalInvoice);
        }

        private Contract InitContract(RentingRequestDto rentingRequest,
            List<Term> contractTerms,
            MachineSerialNumber machineSerialNumber,
            double servicePrice,
            InvoiceDto depositInvoice,
            InvoiceDto rentalInvoice)
        {
            var dateCreate = DateTime.Now;
            int numberOfDays = (rentingRequest.DateEnd - rentingRequest.DateStart).Value.Days + 1;

            var contract = new Contract
            {
                ContractId = GlobalConstant.ContractIdPrefixPattern + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                SerialNumber = machineSerialNumber.SerialNumber,

                DateCreate = dateCreate,
                Status = ContractStatusEnum.NotSigned.ToString(),

                ContractName = GlobalConstant.ContractName + machineSerialNumber.SerialNumber,
                DateStart = rentingRequest.DateStart,
                DateEnd = rentingRequest.DateEnd,
                Content = string.Empty,
                RentingRequestId = rentingRequest.RentingRequestId,
                AccountSignId = rentingRequest.AccountOrderId,
                NumberOfMonth = rentingRequest.NumberOfMonth,
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
            depositContractPayment.InvoiceId = depositInvoice.InvoiceId;
            contractPayments.Add(depositContractPayment);

            if ((bool)rentingRequest.IsOnetimePayment)
            {
                var rentalContractPayment = new ContractPayment
                {
                    ContractId = contract.ContractId,
                    DateCreate = DateTime.Now,
                    Status = ContractPaymentStatusEnum.Pending.ToString(),
                    Type = ContractPaymentTypeEnum.Rental.ToString(),
                    Title = "Thanh toán tiền thuê cho hợp đồng " + contract.ContractId,
                    Amount = (contract.RentPrice * contract.RentPeriod) + servicePrice,
                    DateFrom = contract.DateStart,
                    DateTo = contract.DateEnd,
                    Period = contract.RentPeriod,
                    DueDate = contract.DateStart,
                    IsFirstRentalPayment = true,
                };

                rentalContractPayment.InvoiceId = rentalInvoice.InvoiceId;

                contractPayments.Add(rentalContractPayment);
            }
            else
            {
                var currentStartDate = rentingRequest.DateStart;
                var remainingDays = numberOfDays;

                int paymentIndex = 1;
                while (remainingDays > 0)
                {
                    var paymentEndDate = GetContractPaymentEndDate((DateTime)currentStartDate, (DateTime)rentingRequest.DateEnd);

                    var paymentPeriod = (paymentEndDate - currentStartDate).Value.Days + 1;
                    var paymentAmount = contract.RentPrice * paymentPeriod;

                    var rentalContractPayment = new ContractPayment
                    {
                        ContractId = contract.ContractId,
                        DateCreate = DateTime.Now,
                        Status = ContractPaymentStatusEnum.Pending.ToString(),
                        Type = ContractPaymentTypeEnum.Rental.ToString(),
                        Title = $"Thanh toán tiền thuê cho hợp đồng {contract.ContractId} - Lần {paymentIndex}",
                        Amount = paymentAmount,
                        DateFrom = currentStartDate,
                        DateTo = paymentEndDate,
                        Period = paymentPeriod,
                        DueDate = currentStartDate,
                        IsFirstRentalPayment = false,
                    };

                    if (paymentIndex == 1)
                    {
                        rentalContractPayment.Amount += servicePrice;
                        rentalContractPayment.IsFirstRentalPayment = true;

                        rentalContractPayment.InvoiceId = rentalInvoice.InvoiceId;
                        rentalInvoice.Amount += rentalContractPayment.Amount;
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
                Title = "Thanh toán tiền đặt cọc cho hợp đồng " + contract.ContractId,
                Amount = contract.DepositPrice,
                DateFrom = contract.DateStart,
                DateTo = contract.DateEnd,
                Period = contract.RentPeriod,
                DueDate = contract.DateStart,
                IsFirstRentalPayment = false,
            };

            return contractPaymentDeposit;
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

        public async Task<ContractDto> GetContractById(string id)
        {
            var contracts = await ContractDao.Instance.GetAllAsync();

            var contract = contracts.FirstOrDefault(c => c.ContractId == id);

            if (contract == null)
            {
                return null;
            }

            return _mapper.Map<ContractDto>(contract);
        }

        public async Task UpdateContractStatus(string contractId, string status)
        {
            var contract = await ContractDao.Instance.GetContractById(contractId);

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

        public async Task ScheduleNextRentalPayment(string rentingRequestId)
        {
            await ContractPaymentDao.Instance.ScheduleNextRentalPayment(rentingRequestId);
        }

        public async Task<IEnumerable<ContractDto>> GetContractListOfRequest(string rentingRequestId)
        {
            var contractList = await ContractDao.Instance.GetContractsByRentingRequestId(rentingRequestId);

            return _mapper.Map<IEnumerable<ContractDto>>(contractList);
        }
    }
}
