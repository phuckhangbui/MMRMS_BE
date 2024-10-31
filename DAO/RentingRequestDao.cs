using BusinessObject;
using Common;
using Common.Enum;
using DTOs.RentingRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAO
{
    public class RentingRequestDao : BaseDao<RentingRequest>
    {
        private static RentingRequestDao instance = null;
        private static readonly object instacelock = new object();

        private RentingRequestDao()
        {

        }

        public static RentingRequestDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RentingRequestDao();
                }
                return instance;
            }
        }

        public async Task<RentingRequest> GetRentingRequestById(string rentingRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .Include(rr => rr.ServiceRentingRequests)
                        .ThenInclude(rr => rr.RentingService)
                    .Include(rr => rr.AccountOrder)
                    .Include(rr => rr.Contracts)
                        .ThenInclude(c => c.ContractMachineSerialNumber)
                        .ThenInclude(s => s.Machine)
                        .ThenInclude(p => p.MachineImages)
                    .Include(rr => rr.RentingRequestAddress)
                    .FirstOrDefaultAsync(rr => rr.RentingRequestId.Equals(rentingRequestId));
            }
        }

        public async Task<IEnumerable<RentingRequest>> GetRentingRequests()
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests.Include(h => h.AccountOrder).ToListAsync();
            }
        }

        public async Task<IEnumerable<RentingRequest>> GetRentingRequestsForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.RentingRequests
                    .Where(rr => rr.AccountOrderId == customerId)
                    .Include(h => h.AccountOrder)
                    .OrderByDescending(rr => rr.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<RentingRequest> CreateRentingRequest(RentingRequest rentingRequest, NewRentingRequestDto newRentingRequestDto)
        {
            using var context = new MmrmsContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                double totalDepositPrice = 0;
                double totalRentPrice = 0;

                //Invoice
                var depositInvoice = InitDepositInvoice(rentingRequest);
                var rentalInvoice = InitRentalInvoice(rentingRequest);

                //Service
                double totalServicePricePerContract = (double)rentingRequest.ServiceRentingRequests.Select(s => s.ServicePrice).Sum();

                //Contract
                foreach (var newRentingRequestMachine in newRentingRequestDto.RentingRequestMachineDetails)
                {
                    var availableMachineSerialNumbers = await context.MachineSerialNumbers
                        .Where(s => s.MachineId == newRentingRequestMachine.MachineId && s.Status!.Equals(MachineSerialNumberStatusEnum.Available.ToString()))
                        .Include(s => s.Machine)
                            .ThenInclude(p => p.MachineTerms)
                        .OrderByDescending(s => s.DateCreate)
                        .ToListAsync();

                    //Check serial number in active/signed contract
                    var serialNumbersInFutureContracts = await context.Contracts
                        .Where(c => availableMachineSerialNumbers.Contains(c.ContractMachineSerialNumber)
                                && (c.Status == ContractStatusEnum.NotSigned.ToString() ||
                                    c.Status == ContractStatusEnum.Signed.ToString() ||
                                    c.Status == ContractStatusEnum.Shipping.ToString() ||
                                    c.Status == ContractStatusEnum.Renting.ToString())
                                && (c.DateStart < rentingRequest.DateEnd && c.DateEnd > rentingRequest.DateStart))
                        .Select(c => c.ContractMachineSerialNumber)
                        .ToListAsync();

                    var suitableAvailableSerialNumbers = availableMachineSerialNumbers
                        .Except(serialNumbersInFutureContracts)
                        .ToList();

                    var selectedSerialNumbers = suitableAvailableSerialNumbers
                        .Take(newRentingRequestMachine.Quantity)
                        .ToList();

                    var contractTerms = await context.Terms
                        .Where(t => t.Type.Equals(TermTypeEnum.Contract.ToString()))
                        .ToListAsync();

                    foreach (var machineSerialNumber in selectedSerialNumbers)
                    {
                        //Assign serial number to the contract
                        var contractSerialNumber = InitContract(machineSerialNumber, rentingRequest, contractTerms, depositInvoice, rentalInvoice, totalServicePricePerContract);
                        totalDepositPrice += (double)contractSerialNumber.DepositPrice!;
                        totalRentPrice += (double)contractSerialNumber.TotalRentPrice!;

                        rentingRequest.Contracts.Add(contractSerialNumber);
                    }
                }

                rentingRequest.TotalRentPrice = totalRentPrice;
                rentingRequest.TotalDepositPrice = totalDepositPrice;
                rentingRequest.TotalServicePrice = totalServicePricePerContract * rentingRequest.Contracts.Count;
                rentingRequest.TotalAmount = rentingRequest.TotalDepositPrice + rentingRequest.TotalServicePrice
                    + rentingRequest.TotalRentPrice + rentingRequest.ShippingPrice
                    - rentingRequest.DiscountPrice;

                //Invoice amount
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

                DbSet<RentingRequest> _dbSet = context.Set<RentingRequest>();
                _dbSet.Add(rentingRequest);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                //Background
                ILogger<BackgroundImpl> logger = new LoggerFactory().CreateLogger<BackgroundImpl>();
                var backgroundImpl = new BackgroundImpl(logger);
                backgroundImpl.CancelRentingRequestJob(rentingRequest.RentingRequestId);

                return rentingRequest;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }

        private Contract InitContract(
            MachineSerialNumber machineSerialNumber,
            RentingRequest rentingRequest,
            List<Term> contractTerms,
            Invoice depositInvoice,
            Invoice rentalInvoice,
            double servicePrice)
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

            //DepositContractPayment
            var contractPayments = new List<ContractPayment>();
            var depositContractPayment = InitDepositContractPayment(contract);
            depositContractPayment.Invoice = depositInvoice;
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

                rentalContractPayment.Invoice = rentalInvoice;

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

                        rentalContractPayment.Invoice = rentalInvoice;
                        rentalInvoice.Amount += rentalContractPayment.Amount;
                    }

                    contractPayments.Add(rentalContractPayment);

                    // Move to the next payment period, start the next period the day after
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

        private Invoice InitDepositInvoice(RentingRequest rentingRequest)
        {
            var invoice = new Invoice
            {
                InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "DEPOSIT" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                Amount = rentingRequest.TotalDepositPrice,
                Type = InvoiceTypeEnum.Deposit.ToString(),
                Status = InvoiceStatusEnum.Pending.ToString(),
                DateCreate = DateTime.Now,
                AccountPaidId = rentingRequest.AccountOrderId,
            };

            return invoice;
        }

        private Invoice InitRentalInvoice(RentingRequest rentingRequest)
        {
            var invoice = new Invoice
            {
                InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "RENTAL" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                Amount = 0,
                Type = InvoiceTypeEnum.Rental.ToString(),
                Status = InvoiceStatusEnum.Pending.ToString(),
                DateCreate = DateTime.Now,
                AccountPaidId = rentingRequest.AccountOrderId,
            };

            return invoice;
        }

        public async Task<RentingRequest?> CancelRentingRequest(string rentingRequestId)
        {
            using var context = new MmrmsContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var rentingRequest = await context.RentingRequests
                    .Include(rr => rr.Contracts)
                        .ThenInclude(c => c.ContractPayments)
                        .ThenInclude(cp => cp.Invoice)
                    .FirstOrDefaultAsync(rq => rq.RentingRequestId.Equals(rentingRequestId));

                if (rentingRequest != null)
                {
                    rentingRequest.Status = RentingRequestStatusEnum.Canceled.ToString();

                    foreach (var contract in rentingRequest.Contracts)
                    {
                        contract.Status = ContractStatusEnum.Canceled.ToString();

                        foreach (var contractPayment in contract.ContractPayments)
                        {
                            contractPayment.Status = ContractPaymentStatusEnum.Canceled.ToString();

                            if (contractPayment.Invoice != null)
                            {
                                contractPayment.Invoice.Status = InvoiceStatusEnum.Canceled.ToString();
                            }
                        }

                        context.Contracts.Update(contract);
                    }

                    context.RentingRequests.Update(rentingRequest);

                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }

                return rentingRequest;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }
    }
}
