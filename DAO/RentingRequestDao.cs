using BusinessObject;
using Common;
using Common.Enum;
using DTOs.RentingRequest;
using Microsoft.EntityFrameworkCore;
using Contract = BusinessObject.Contract;
using MachineSerialNumber = BusinessObject.MachineSerialNumber;
using RentingRequest = BusinessObject.RentingRequest;

namespace DAO
{
    public class RentingRequestDao : BaseDao<RentingRequest>
    {
        private static RentingRequestDao instance = null;
        private static readonly object instacelock = new object();

        public RentingRequestDao()
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

        //public async Task<RentingRequest> GetRentingRequestByIdAndStatus(string rentingRequestId, string status)
        //{
        //    using (var context = new MmrmsContext())
        //    {
        //        return await context.RentingRequests
        //            .FirstOrDefaultAsync(h => h.RentingRequestId.Equals(rentingRequestId)
        //                && h.Status.Equals(status));
        //    }
        //}

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
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        double totalDepositPrice = 0;
                        double totalRentPrice = 0;
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
                            //var requestedEndDate = rentingRequest.DateStart!.Value.AddMonths((int)rentingRequest.NumberOfMonth!);
                            var serialNumbersInFutureContracts = await context.Contracts
                                .Where(c => availableMachineSerialNumbers.Contains(c.ContractMachineSerialNumber)
                                        && (c.Status == ContractStatusEnum.NotSigned.ToString() || c.Status == ContractStatusEnum.Signed.ToString() || c.Status == ContractStatusEnum.Renting.ToString())
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
                                var contractSerialNumber = InitContract(machineSerialNumber, rentingRequest, contractTerms);
                                totalDepositPrice += (double)contractSerialNumber.DepositPrice!;
                                totalRentPrice += (double)contractSerialNumber.TotalRentPrice!;

                                rentingRequest.Contracts.Add(contractSerialNumber);
                            }
                        }

                        rentingRequest.TotalRentPrice = totalRentPrice;
                        rentingRequest.TotalDepositPrice = totalDepositPrice;
                        rentingRequest.TotalAmount = rentingRequest.TotalDepositPrice + rentingRequest.TotalServicePrice
                            + rentingRequest.TotalRentPrice + rentingRequest.ShippingPrice
                            - rentingRequest.DiscountPrice;

                        DbSet<RentingRequest> _dbSet = context.Set<RentingRequest>();
                        _dbSet.Add(rentingRequest);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

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

        private Contract InitContract(MachineSerialNumber machineSerialNumber, RentingRequest rentingRequest, List<Term> contractTerms)
        {
            var dateCreate = DateTime.Now;
            int numberOfDays = (rentingRequest.DateEnd - rentingRequest.DateStart).Value.Days;

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

            //ContractPayment
            var contractPayments = new List<ContractPayment>();
            var depositContractPayment = InitDepositContractPayment(contract);
            contractPayments.Add(depositContractPayment);

            if ((bool)rentingRequest.IsOnetimePayment)
            {
                var rentalContractPayment = new ContractPayment
                {
                    ContractId = contract.ContractId,
                    DateCreate = DateTime.Now.Date,
                    Status = ContractPaymentStatusEnum.Pending.ToString(),
                    Type = ContractPaymentTypeEnum.Rental.ToString(),
                    Title = "Thanh toán tiền thuê cho hợp đồng " + contract.ContractId,
                    Amount = contract.RentPrice * contract.RentPeriod,
                    DateFrom = contract.DateStart,
                    DateTo = contract.DateEnd,
                    Period = contract.RentPeriod,
                    DueDate = contract.DateStart,
                    IsFirstRentalPayment = true,
                };

                contractPayments.Add(rentalContractPayment);

                contract.ContractPayments = contractPayments;
            }

            return contract;
        }

        private ContractPayment InitDepositContractPayment(Contract contract)
        {
            var contractPaymentDeposit = new ContractPayment
            {
                ContractId = contract.ContractId,
                DateCreate = DateTime.Now.Date,
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

        public async Task<RentingRequest?> CancelRentingRequest(string rentingRequestId)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var rentingRequest = await context.RentingRequests
                            .Include(rr => rr.Contracts)
                                .ThenInclude(c => c.ContractPayments)
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
    }
}
