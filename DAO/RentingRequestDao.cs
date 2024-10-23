using BusinessObject;
using Common;
using Common.Enum;
using DTOs.RentingRequest;
using Microsoft.EntityFrameworkCore;
using Contract = BusinessObject.Contract;
using RentingRequest = BusinessObject.RentingRequest;
using MachineSerialNumber = BusinessObject.MachineSerialNumber;

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
                        //Promotion
                        //if (accountPromotionId != 0)
                        //{
                        //    var accountPromotion = await context.AccountPromotions
                        //        .FirstOrDefaultAsync(ap => ap.AccountPromotionId == accountPromotionId);

                        //    if (accountPromotion != null && accountPromotion.Status!.Equals(AccountPromotionStatusEnum.Active.ToString()))
                        //    {
                        //        accountPromotion.Status = AccountPromotionStatusEnum.Redeemed.ToString();
                        //        context.AccountPromotions.Update(accountPromotion);
                        //    }
                        //}

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
                            var requestedEndDate = rentingRequest.DateStart!.Value.AddMonths((int)rentingRequest.NumberOfMonth!);
                            var serialNumbersInFutureContracts = await context.Contracts
                                .Where(c => availableMachineSerialNumbers.Contains(c.ContractMachineSerialNumber)
                                        && (c.Status == ContractStatusEnum.NotSigned.ToString() || c.Status == ContractStatusEnum.Signed.ToString() || c.Status == ContractStatusEnum.Shipping.ToString() || c.Status == ContractStatusEnum.Shipped.ToString())
                                        && (c.DateStart < requestedEndDate && c.DateEnd > rentingRequest.DateStart))
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

                                //Update machineSerialNumber
                                //machineSerialNumber.Status = MachineSerialNumberStatusEnum.Rented.ToString();
                                //machineSerialNumber.RentTimeCounter++;
                                //context.MachineSerialNumbers.Update(machineSerialNumber);

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

            var contractSerialNumber = new Contract
            {
                ContractId = GlobalConstant.ContractIdPrefixPattern + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern) + machineSerialNumber.SerialNumber,
                SerialNumber = machineSerialNumber.SerialNumber,

                DateCreate = dateCreate,
                Status = ContractStatusEnum.NotSigned.ToString(),

                ContractName = GlobalConstant.ContractName + machineSerialNumber.SerialNumber,
                DateStart = rentingRequest.DateStart,
                DateEnd = rentingRequest.DateStart!.Value.AddMonths((int)rentingRequest.NumberOfMonth!),
                Content = string.Empty,
                RentingRequestId = rentingRequest.RentingRequestId,
                AccountSignId = rentingRequest.AccountOrderId,
                NumberOfMonth = rentingRequest.NumberOfMonth,

                RentPrice = machineSerialNumber.ActualRentPrice,
                DepositPrice = machineSerialNumber.Machine!.MachinePrice * GlobalConstant.DepositValue,
                TotalRentPrice = machineSerialNumber.ActualRentPrice * rentingRequest.NumberOfMonth,
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

                    contractSerialNumber.ContractTerms.Add(term);
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

                    contractSerialNumber.ContractTerms.Add(term);
                }
            }

            return contractSerialNumber;
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
                            .FirstOrDefaultAsync(rq => rq.RentingRequestId.Equals(rentingRequestId));

                        if (rentingRequest != null)
                        {
                            rentingRequest.Status = RentingRequestStatusEnum.Canceled.ToString();

                            foreach (var contract in rentingRequest.Contracts)
                            {
                                contract.Status = ContractStatusEnum.Canceled.ToString();
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
