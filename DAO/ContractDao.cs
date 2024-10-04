using BusinessObject;
using Common;
using Common.Enum;
using DTOs.Contract;
using Microsoft.EntityFrameworkCore;
using Contract = BusinessObject.Contract;

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

                            //TODO
                            var contractSerialNumberProduct = new ContractSerialNumberProduct()
                            {
                                SerialNumber = rentSerialNumberProduct.SerialNumber,
                                DepositPrice = serialNumberProduct!.Product!.ProductPrice * GlobalConstant.DepositValue,
                                RentPrice = serialNumberProduct!.ActualRentPrice ?? 0,
                            };
                            contract.ContractSerialNumberProducts.Add(contractSerialNumberProduct);
                            totalDepositPrice += (double)contractSerialNumberProduct.DepositPrice!;
                            totalRentPrice += (double)contractSerialNumberProduct.RentPrice;

                            serialNumberProduct!.Status = SerialNumberProductStatusEnum.Rented.ToString();
                            serialNumberProduct.RentTimeCounter++;

                            context.SerialNumberProducts.Update(serialNumberProduct);
                        }

                        var rentingRequest = await context.RentingRequests.FirstOrDefaultAsync(rq => rq.RentingRequestId.Equals(contract.RentingRequestId));

                        //TODO
                        contract.TotalDepositPrice = totalDepositPrice;
                        contract.TotalRentPrice = totalRentPrice;
                        contract.ShippingPrice = rentingRequest!.ShippingPrice;
                        contract.DiscountPrice = rentingRequest.DiscountPrice;
                        contract.DiscountShip = rentingRequest!.DiscountShip;
                        contract.FinalAmount = contract.TotalDepositPrice + contract.TotalRentPrice + contract.ShippingPrice
                            - contract.DiscountPrice - contract.DiscountShip;

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
