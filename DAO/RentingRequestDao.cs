using BusinessObject;
using Common;
using Common.Enum;
using DTOs.RentingRequest;
using Microsoft.EntityFrameworkCore;
using Contract = BusinessObject.Contract;
using RentingRequest = BusinessObject.RentingRequest;
using SerialNumberProduct = BusinessObject.SerialNumberProduct;

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
                    .Include(rr => rr.RentingRequestProductDetails)
                        .ThenInclude(rr => rr.Product)
                    .Include(rr => rr.ServiceRentingRequests)
                        .ThenInclude(rr => rr.RentingService)
                    .Include(rr => rr.AccountOrder)
                    .Include(rr => rr.Contracts)
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
                        foreach (var newRentingRequestProduct in newRentingRequestDto.RentingRequestProductDetails)
                        {
                            var availableSerialNumberProducts = await context.SerialNumberProducts
                                .Where(s => s.ProductId == newRentingRequestProduct.ProductId && s.Status!.Equals(SerialNumberProductStatusEnum.Available.ToString()))
                                .Include(s => s.Product)
                                    .ThenInclude(p => p.ProductTerms)
                                .OrderByDescending(s => s.DateCreate)
                                .ToListAsync();

                            //Check serial number in active/signed contract
                            var requestedEndDate = rentingRequest.DateStart!.Value.AddMonths((int)rentingRequest.NumberOfMonth!);
                            var serialNumbersInFutureContracts = await context.Contracts
                                .Where(c => availableSerialNumberProducts.Contains(c.ContractSerialNumberProduct)
                                        && (c.Status == ContractStatusEnum.Active.ToString() || c.Status == ContractStatusEnum.Signed.ToString())
                                        && (c.DateStart < requestedEndDate && c.DateEnd > rentingRequest.DateStart))
                                .Select(c => c.ContractSerialNumberProduct)
                                .ToListAsync();

                            var suitableAvailableSerialNumbers = availableSerialNumberProducts
                                .Except(serialNumbersInFutureContracts)
                                .ToList();

                            var selectedSerialNumbers = suitableAvailableSerialNumbers
                                .Take(newRentingRequestProduct.Quantity)
                                .ToList();

                            var contractTerms = await context.Terms
                                .Where(t => t.Type.Equals(TermTypeEnum.Contract.ToString()))
                                .ToListAsync();

                            foreach (var serialNumberProduct in selectedSerialNumbers)
                            {
                                //Assign serial number to the contract
                                var contractSerialNumber = InitContract(serialNumberProduct, rentingRequest, contractTerms);
                                totalDepositPrice += (double)contractSerialNumber.DepositPrice!;
                                totalRentPrice += (double)contractSerialNumber.TotalRentPrice!;

                                //Update serialNumberProduct
                                //serialNumberProduct.Status = SerialNumberProductStatusEnum.Rented.ToString();
                                //serialNumberProduct.RentTimeCounter++;
                                //context.SerialNumberProducts.Update(serialNumberProduct);

                                rentingRequest.Contracts.Add(contractSerialNumber);
                            }
                        }

                        rentingRequest.TotalRentPrice = totalRentPrice;
                        rentingRequest.TotalDepositPrice = totalDepositPrice;
                        rentingRequest.TotalAmount =
                            rentingRequest.TotalAmount + rentingRequest.TotalDepositPrice + rentingRequest.TotalServicePrice
                            + rentingRequest.TotalRentPrice + rentingRequest.ShippingPrice
                            - rentingRequest.DiscountPrice - rentingRequest.DiscountShip;

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

        private Contract InitContract(SerialNumberProduct serialNumberProduct, RentingRequest rentingRequest, List<Term> contractTerms)
        {
            var dateCreate = DateTime.Now.Date;

            var contractSerialNumber = new Contract
            {
                ContractId = GlobalConstant.ContractIdPrefixPattern + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern) + serialNumberProduct.SerialNumber,
                SerialNumber = serialNumberProduct.SerialNumber,

                DateCreate = dateCreate,
                Status = ContractStatusEnum.NotSigned.ToString(),

                ContractName = GlobalConstant.ContractName + serialNumberProduct.SerialNumber,
                DateStart = rentingRequest.DateStart,
                DateEnd = rentingRequest.DateStart!.Value.AddMonths((int)rentingRequest.NumberOfMonth!),
                Content = string.Empty,
                RentingRequestId = rentingRequest.RentingRequestId,
                AccountSignId = rentingRequest.AccountOrderId,
                NumberOfMonth = rentingRequest.NumberOfMonth,

                RentPrice = serialNumberProduct.ActualRentPrice,
                DepositPrice = serialNumberProduct.Product!.ProductPrice * GlobalConstant.DepositValue,
                TotalRentPrice = serialNumberProduct.ActualRentPrice * rentingRequest.NumberOfMonth,
            };

            //Contract Term
            foreach (var productTerm in serialNumberProduct.Product.ProductTerms)
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
    }
}
