using BusinessObject;
using Common;
using Common.Enum;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Contract> GetContractDetailById(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                     .Include(c => c.RentingRequest)
                        .ThenInclude(r => r.RentingRequestAddress)
                     .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusiness)
                    .Include(c => c.ContractTerms)
                    .Include(c => c.ContractPayments)
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(a => a.Machine)
                        .ThenInclude(m => m.MachineImages)
                    .FirstOrDefaultAsync(c => c.ContractId == contractId);
            }
        }

        public async Task<Contract?> GetContractById(string contractId)
        {
            using var context = new MmrmsContext();
            return await context.Contracts
                .Include(c => c.ContractPayments)
                .Include(c => c.ContractMachineSerialNumber)
                .FirstOrDefaultAsync(c => c.ContractId == contractId);
        }

        public async Task<IEnumerable<Contract>> GetContractsByRentingRequestId(string rentingRequestId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Where(c => c.RentingRequestId.Equals(rentingRequestId))
                     .Include(c => c.RentingRequest)
                        .ThenInclude(r => r.RentingRequestAddress)
                     .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusiness)
                    .Include(c => c.ContractTerms)
                    .Include(c => c.ContractPayments)
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(a => a.Machine)
                        .ThenInclude(m => m.MachineImages)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetContractsForCustomer(int customerId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Where(c => c.AccountSignId == customerId)
                    .Include(c => c.RentingRequest)
                        .ThenInclude(r => r.RentingRequestAddress)
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(a => a.Machine)
                        .ThenInclude(m => m.MachineImages)
                    .OrderByDescending(c => c.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetContracts()
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                    .Include(c => c.RentingRequest)
                        .ThenInclude(r => r.RentingRequestAddress)
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(s => s.Machine)
                    .ThenInclude(m => m.MachineImages)
                    .OrderByDescending(c => c.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Contract>> GetRentalHistoryOfSerialNumber(string serialNumber)
        {
            using var context = new MmrmsContext();
            return await context.Contracts
                .Where(c => c.SerialNumber.Equals(serialNumber))
                .Include(c => c.ContractMachineSerialNumber)
                    .ThenInclude(s => s.Machine)
                .ThenInclude(m => m.MachineImages)
                .OrderByDescending(c => c.DateCreate)
                .ToListAsync();
        }

        public async Task<RentingRequestAddress?> GetRentingRequestAddressByContractId(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                var contract = await context.Contracts.FirstOrDefaultAsync(c => c.ContractId == contractId);

                if (contract == null)
                {
                    return null;
                }

                var rentingRequest = await context.RentingRequests.Include(r => r.RentingRequestAddress).FirstOrDefaultAsync(r => r.RentingRequestId == contract.RentingRequestId);

                if (rentingRequest == null)
                {
                    return null;
                }

                return rentingRequest.RentingRequestAddress;
            }
        }

        public async Task<int> GetTotalContractByDate(DateTime date)
        {
            using var context = new MmrmsContext();
            return await context.Contracts
                .Where(r => r.DateCreate.HasValue && r.DateCreate.Value.Date == date.Date)
                .CountAsync();
        }

        public async Task<int> GetContractsInRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            using var context = new MmrmsContext();
            IQueryable<Contract> query = context.Contracts;

            if (startDate.HasValue)
            {
                query = query.Where(a => a.DateCreate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.DateCreate <= endDate.Value);
            }

            return await query.CountAsync();
        }

        public async Task<Contract?> GetExtendContract(string baseContractId)
        {
            using var context = new MmrmsContext();
            return await context.Contracts
                .FirstOrDefaultAsync(c => c.BaseContractId.Equals(baseContractId));
        }

        public async Task<int> GetLastestContractByDate(DateTime date)
        {
            using (var context = new MmrmsContext())
            {
                var ids = await context.Contracts
                    .Where(t => t.DateCreate.HasValue && t.DateCreate.Value.Date == date.Date)
                    .Where(t => t.ContractId != null)
                    .Select(t => t.ContractId)
                    .ToListAsync();

                if (!ids.Any())
                {
                    return 0;
                }

                var latestId = ids
                     .Select(id =>
                     {
                         int startIndex = id.IndexOf("NO") + 2;
                         return int.Parse(id.Substring(startIndex));
                     })
                     .OrderByDescending(id => id)
                     .FirstOrDefault();

                return latestId;
            }
        }

        public async Task<IEnumerable<Contract>> GetContractBySerialNumber(string serialNumber)
        {
            using (var context = new MmrmsContext())
            {
                var list = await context.Contracts.Where(c => c.SerialNumber == serialNumber).ToListAsync();
                return list;
            }
        }

        public async Task UpdateContractWhenSignedExtendContract(string extendContractId)
        {
            using var context = new MmrmsContext();
            try
            {
                var extendContract = await context.Contracts
                    .Include(c => c.ContractPayments)
                    .ThenInclude(cp => cp.Invoice)
                    .FirstOrDefaultAsync(c => c.ContractId == extendContractId);

                if (extendContract != null)
                {
                    var baseContractId = extendContract.BaseContractId;
                    var baseContract = await context.Contracts
                        .Include(c => c.ContractPayments)
                        .FirstOrDefaultAsync(c => c.ContractId.Equals(baseContractId));

                    if (baseContract != null)
                    {
                        extendContract.DepositPrice = baseContract.DepositPrice;
                        extendContract.RefundShippingPrice = baseContract.RefundShippingPrice;

                        foreach (var contractPayment in baseContract.ContractPayments)
                        {
                            if (contractPayment.Type.Equals(ContractPaymentTypeEnum.Deposit.ToString()))
                            {
                                contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();

                                var depositContractPayment = new ContractPayment
                                {
                                    ContractId = extendContractId,
                                    DateCreate = DateTime.Now,
                                    Status = ContractPaymentStatusEnum.Paid.ToString(),
                                    Type = ContractPaymentTypeEnum.Deposit.ToString(),
                                    Title = GlobalConstant.DepositContractPaymentTitle + extendContractId,
                                    Amount = contractPayment.Amount,
                                    DateFrom = extendContract.DateStart,
                                    DateTo = extendContract.DateEnd,
                                    Period = extendContract.RentPeriod,
                                    DueDate = extendContract.DateStart,
                                    IsFirstRentalPayment = false,
                                };
                                extendContract.ContractPayments.Add(depositContractPayment);
                            }

                            if (contractPayment.Type.Equals(ContractPaymentTypeEnum.Refund.ToString()))
                            {
                                contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();

                                var refundContractPayment = new ContractPayment
                                {
                                    ContractId = extendContractId,
                                    DateCreate = DateTime.Now,
                                    Status = ContractPaymentStatusEnum.Pending.ToString(),
                                    Type = ContractPaymentTypeEnum.Refund.ToString(),
                                    Title = GlobalConstant.RefundContractPaymentTitle + extendContractId,
                                    Amount = contractPayment.Amount,
                                    DateFrom = extendContract.DateEnd,
                                    DateTo = extendContract.DateEnd,
                                    Period = extendContract.RentPeriod,
                                    DueDate = extendContract.DateEnd,
                                    IsFirstRentalPayment = false,
                                };
                                extendContract.ContractPayments.Add(refundContractPayment);
                            }

                            if (contractPayment.Type.Equals(ContractPaymentTypeEnum.Fine.ToString()))
                            {
                                contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();

                                var fineContractPayment = new ContractPayment
                                {
                                    ContractId = extendContractId,
                                    DateCreate = DateTime.Now,
                                    Status = ContractPaymentStatusEnum.Pending.ToString(),
                                    Type = ContractPaymentTypeEnum.Fine.ToString(),
                                    Title = GlobalConstant.FineContractPaymentTitle + extendContractId,
                                    Amount = contractPayment.Amount,
                                    DateFrom = contractPayment.DateFrom,
                                    DateTo = contractPayment.DateTo,
                                    Period = contractPayment.Period,
                                    DueDate = contractPayment.DueDate,
                                    IsFirstRentalPayment = false,
                                };
                                extendContract.ContractPayments.Add(fineContractPayment);
                            }
                        }

                        context.Contracts.Update(extendContract);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CancelExtendContract(string extendContractId)
        {
            using var context = new MmrmsContext();
            try
            {
                var extendContract = await context.Contracts
                    .Include(c => c.ContractPayments)
                    .ThenInclude(cp => cp.Invoice)
                    .FirstOrDefaultAsync(c => c.ContractId == extendContractId);

                if (extendContract != null)
                {
                    var baseContractId = extendContract.BaseContractId;
                    extendContract.BaseContractId = null;
                    extendContract.Status = ContractStatusEnum.Canceled.ToString();

                    foreach (var contractPayment in extendContract.ContractPayments)
                    {
                        contractPayment.Status = ContractPaymentStatusEnum.Canceled.ToString();

                        if (contractPayment.Invoice != null)
                        {
                            contractPayment.Invoice.Status = InvoiceStatusEnum.Canceled.ToString();
                        }
                    }

                    context.Contracts.Update(extendContract);

                    var baseContract = await context.Contracts
                        .FirstOrDefaultAsync(c => c.ContractId.Equals(baseContractId));

                    if (baseContract != null)
                    {
                        baseContract.IsExtended = false;
                        context.Contracts.Update(extendContract);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
