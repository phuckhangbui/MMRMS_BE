using BusinessObject;
using Common.Enum;
using DTOs.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DAO
{
    public class ContractDao : BaseDao<Contract>
    {
        private static ContractDao instance = null;
        private static readonly object instacelock = new object();

        private ContractDao()
        {

        }

        public static ContractDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContractDao();
                }
                return instance;
            }
        }

        public async Task<Contract> GetContractById(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                     .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusiness)
                    .Include(c => c.ContractTerms)
                    .FirstOrDefaultAsync(c => c.ContractId == contractId);
            }
        }

        public async Task<IEnumerable<Contract>> GetContractsForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Where(c => c.AccountSignId == customerId)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetContracts()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusiness)
                    .Include(c => c.ContractTerms)
                    .ToListAsync();
            }
        }

        //TODO: ContractPayment (DueDate)
        public async Task SignContract(string rentingRequestId)
        {
            using var context = new MmrmsContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var rentingRequest = await context.RentingRequests
                    .Include(rr => rr.Contracts)
                    .FirstOrDefaultAsync(rq => rq.RentingRequestId.Equals(rentingRequestId));

                if (rentingRequest != null && !rentingRequest.Contracts.IsNullOrEmpty())
                {
                    foreach (var contract in rentingRequest.Contracts)
                    {
                        var contractPayments = new List<ContractPayment>();

                        //IsOnetimePayment: true
                        if ((bool)rentingRequest.IsOnetimePayment!)
                        {
                            //ContractPayment(Deposit)
                            var contractPaymentDeposit = InitContractPaymentDeposit(contract);

                            //ContractPayment(Rental)
                            var contractPaymentRental = new ContractPayment
                            {
                                ContractId = contract.ContractId,
                                DateCreate = DateTime.Now.Date,
                                Status = ContractPaymentStatusEnum.Pending.ToString(),
                                Type = ContractPaymentTypeEnum.Rental.ToString(),
                                Title = "Thanh toán tiền thuê cho hợp đồng " + contract.ContractId,
                                Amount = (contract.RentPrice * contract.NumberOfMonth) +
                                    rentingRequest.ShippingPrice - rentingRequest.DiscountShip - rentingRequest.DiscountPrice +
                                    rentingRequest.TotalServicePrice,
                                DueDate = contract.DateStart,
                            };

                            contractPayments.Add(contractPaymentDeposit);
                            contractPayments.Add(contractPaymentRental);

                            contract.ContractPayments = contractPayments;
                        }
                        //IsOnetimePayment: false
                        else
                        {
                            //ContractPayment(Deposit)
                            var contractPaymentDeposit = InitContractPaymentDeposit(contract);

                            var contractPaymentRental = new ContractPayment
                            {
                                ContractId = contract.ContractId,
                                DateCreate = DateTime.Now.Date,
                                Status = ContractPaymentStatusEnum.Pending.ToString(),
                                Type = ContractPaymentTypeEnum.Rental.ToString(),
                                Title = "Thanh toán tiền thuê cho hợp đồng " + contract.ContractId + " lần 1",
                                Amount = (contract.RentPrice * 3) +
                                    rentingRequest.ShippingPrice - rentingRequest.DiscountShip - rentingRequest.DiscountPrice +
                                    rentingRequest.TotalServicePrice,
                                DueDate = contract.DateStart,
                            };

                            contractPayments.Add(contractPaymentDeposit);
                            contractPayments.Add(contractPaymentRental);

                            var time = (contract.NumberOfMonth / 3) - 1;
                            if (time > 0)
                            {
                                contractPayments.AddRange(InitContractPaymentRentalByTime(contract, (int)time!));
                            }

                            contract.ContractPayments = contractPayments;
                        }

                        contract.Status = ContractStatusEnum.Signed.ToString();

                        context.Contracts.Update(contract);
                    }

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }

        private List<ContractPayment> InitContractPaymentRentalByTime(Contract contract, int time)
        {
            var list = new List<ContractPayment>();
            for (int i = 0; i < time; i++)
            {
                int monthsToAdd = (i + 1) * 3;

                var contractPaymentRental = new ContractPayment
                {
                    ContractId = contract.ContractId,
                    DateCreate = DateTime.Now.Date,
                    Status = ContractPaymentStatusEnum.Pending.ToString(),
                    Type = ContractPaymentTypeEnum.Rental.ToString(),
                    Title = "Thanh toán tiền thuê cho hợp đồng " + contract.ContractId + " lần " + (i + 1),
                    Amount = (contract.RentPrice * 3),
                    DueDate = contract.DateStart!.Value.AddMonths(monthsToAdd),
                };

                list.Add(contractPaymentRental);
            }

            return list;
        }

        private ContractPayment InitContractPaymentDeposit(Contract contract)
        {
            var contractPaymentDeposit = new ContractPayment
            {
                ContractId = contract.ContractId,
                DateCreate = DateTime.Now.Date,
                Status = ContractPaymentStatusEnum.Pending.ToString(),
                Type = ContractPaymentTypeEnum.Deposit.ToString(),
                Title = "Thanh toán tiền đặt cọc cho hợp đồng " + contract.ContractId,
                Amount = contract.DepositPrice,
                DueDate = contract.DateStart,
            };

            return contractPaymentDeposit;
        }


        //TODO: Remove
        public async Task CreateContract(Contract contract, ContractRequestDto contractRequestDto)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        double totalDepositPrice = 0;
                        double totalRentPrice = 0;

                        foreach (var rentSerialNumberProduct in contractRequestDto.SerialNumberProducts)
                        {
                            var serialNumberProduct = await context.SerialNumberProducts
                                .Include(s => s.Product)
                                .FirstOrDefaultAsync(s => s.ProductId == rentSerialNumberProduct.ProductId && s.SerialNumber.Equals(rentSerialNumberProduct.SerialNumber));

                            //var contractSerialNumberProduct = new ContractSerialNumberProduct()
                            //{
                            //    SerialNumber = rentSerialNumberProduct.SerialNumber,
                            //    DepositPrice = serialNumberProduct!.Product!.ProductPrice * GlobalConstant.DepositValue,
                            //    RentPrice = serialNumberProduct!.ActualRentPrice ?? 0,
                            //};
                            //contract.ContractSerialNumberProducts.Add(contractSerialNumberProduct);
                            //totalDepositPrice += (double)contractSerialNumberProduct.DepositPrice!;
                            //totalRentPrice += (double)contractSerialNumberProduct.RentPrice;

                            serialNumberProduct!.Status = SerialNumberProductStatusEnum.Rented.ToString();
                            serialNumberProduct.RentTimeCounter++;

                            context.SerialNumberProducts.Update(serialNumberProduct);
                        }

                        var rentingRequest = await context.RentingRequests.FirstOrDefaultAsync(rq => rq.RentingRequestId.Equals(contract.RentingRequestId));

                        contract.DepositPrice = totalDepositPrice;
                        contract.RentPrice = totalRentPrice;
                        //contract.ShippingPrice = rentingRequest!.ShippingPrice;
                        //contract.DiscountPrice = rentingRequest.DiscountPrice;
                        //contract.DiscountShip = rentingRequest!.DiscountShip;
                        //contract.TotalRentPrice = contract.DepositPrice + contract.RentPrice + contract.ShippingPrice
                        //    - contract.DiscountPrice - contract.DiscountShip;

                        DbSet<Contract> _dbSet = context.Set<Contract>();
                        _dbSet.Add(contract);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();
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
