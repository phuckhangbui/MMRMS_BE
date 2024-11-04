﻿using BusinessObject;
using Common;
using Common.Enum;
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

        public async Task<Contract> GetContractDetailById(string contractId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Contracts
                     .Include(c => c.RentingRequest)
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
                     .Include(c => c.AccountSign)
                        .ThenInclude(a => a.AccountBusiness)
                    .Include(c => c.ContractTerms)
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
                    .Include(c => c.ContractMachineSerialNumber)
                        .ThenInclude(s => s.Machine)
                    .ThenInclude(m => m.MachineImages)
                    .OrderByDescending(c => c.DateCreate)
                    .ToListAsync();
            }
        }

        public async Task UpdateStatusContractsToSignedInRentingRequest(string rentingRequestId, DateTime paymentDate)
        {
            using var context = new MmrmsContext();

            var rentingRequest = await context.RentingRequests
                .Include(r => r.Contracts)
                    .ThenInclude(c => c.ContractPayments)
                .FirstOrDefaultAsync(r => r.RentingRequestId.Equals(rentingRequestId));

            if (rentingRequest != null && !rentingRequest.Contracts.IsNullOrEmpty())
            {
                rentingRequest.Status = RentingRequestStatusEnum.Signed.ToString();

                foreach (var contract in rentingRequest.Contracts)
                {
                    contract.Status = ContractStatusEnum.Signed.ToString();
                    contract.DateSign = paymentDate;

                    await context.SaveChangesAsync();
                }
            }
        }

        //TODO
        public async Task<(Invoice? depositInvoice, Invoice? rentalInvoice)> SignContract(string rentingRequestId)
        {
            using var context = new MmrmsContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var rentingRequest = await context.RentingRequests
                    .Include(rr => rr.Contracts)
                        .ThenInclude(c => c.ContractPayments)
                    .FirstOrDefaultAsync(rq => rq.RentingRequestId.Equals(rentingRequestId));

                if (rentingRequest != null && !rentingRequest.Contracts.IsNullOrEmpty())
                {
                    //var additionalInvoice = InitAdditionalInvoice(rentingRequest);
                    //DbSet<Invoice> _dbSet = context.Set<Invoice>();
                    //_dbSet.Add(additionalInvoice);

                    var depositInvoice = InitDepositInvoiceV2(rentingRequest);
                    Invoice? rentalInvoice = null;
                    //var rentalInvoice = InitRentalInvoiceV2(rentingRequest);
                    DbSet<Invoice> _dbSet = context.Set<Invoice>();
                    _dbSet.Add(depositInvoice);

                    //IsOnetimePayment: true
                    if ((bool)rentingRequest.IsOnetimePayment!)
                    {
                        rentalInvoice = InitRentalInvoiceV2(rentingRequest);

                        foreach (var contract in rentingRequest.Contracts)
                        {
                            //var contractPayments = new List<ContractPayment>();

                            //ContractPayment(Deposit)
                            //var depositContractPayment = InitDepositContractPayment(contract);

                            //ContractPayment(Rental)
                            //var rentalContractPayment = new ContractPayment
                            //{
                            //    ContractId = contract.ContractId,
                            //    DateCreate = DateTime.Now.Date,
                            //    Status = ContractPaymentStatusEnum.Pending.ToString(),
                            //    Type = ContractPaymentTypeEnum.Rental.ToString(),
                            //    Title = "Thanh toán tiền thuê cho hợp đồng " + contract.ContractId,
                            //    Amount = contract.RentPrice * contract.NumberOfMonth,
                            //    DueDate = contract.DateStart,
                            //    IsFirstRentalPayment = true,
                            //};

                            //contractPayments.Add(depositContractPayment);
                            //contractPayments.Add(rentalContractPayment);

                            //contract.ContractPayments = contractPayments;

                            //Deposit Invoice
                            //var depositInvoice = InitDepositInvoice(contract);
                            //depositContractPayment.Invoice = depositInvoice;

                            //Rental Invoice
                            //var rentalInvoice = InitRentalInvoice(contract, rentingRequest);
                            //rentalContractPayment.Invoice = rentalInvoice;

                            var depositContractPayment = contract.ContractPayments.FirstOrDefault(c => c.Type.Equals(ContractPaymentTypeEnum.Deposit.ToString()));
                            depositContractPayment.Invoice = depositInvoice;

                            var rentalContractPayment = contract.ContractPayments.FirstOrDefault(c => c.Type.Equals(ContractPaymentTypeEnum.Rental.ToString()));
                            rentalContractPayment.Invoice = rentalInvoice;

                            context.Contracts.Update(contract);
                        }
                    }
                    //IsOnetimePayment: false
                    else
                    {
                        rentalInvoice = new Invoice
                        {
                            InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "RENTAL" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                            Amount = 0,
                            Type = InvoiceTypeEnum.Rental.ToString(),
                            Status = InvoiceStatusEnum.Pending.ToString(),
                            DateCreate = DateTime.Now,
                            AccountPaidId = rentingRequest.AccountOrderId,
                        };

                        foreach (var contract in rentingRequest.Contracts)
                        {
                            //var contractPayments = new List<ContractPayment>();

                            //ContractPayment(Deposit)
                            //var depositContractPayment = InitDepositContractPayment(contract);
                            //contractPayments.Add(depositContractPayment);

                            //var time = contract.NumberOfMonth / 3;
                            //if (time > 0)
                            //{
                            //    contractPayments.AddRange(InitRentalContractPaymentByTime(contract, (int)time!, rentingRequest, invoice));
                            //}

                            //contract.ContractPayments = contractPayments;

                            //Deposit Invoice
                            //var depositInvoice = InitDepositInvoice(contract);
                            //depositContractPayment.Invoice = depositInvoice;

                            //Rental Invoice
                            //var rentalInvoice = InitRentalInvoice(contract, rentingRequest);
                            //rentalContractPayment.Invoice = rentalInvoice;

                            //contract.Status = ContractStatusEnum.Signed.ToString();
                            //contract.DateSign = DateTime.Now;


                            var depositContractPayment = contract.ContractPayments.FirstOrDefault(c => c.Type.Equals(ContractPaymentTypeEnum.Deposit.ToString()));
                            depositContractPayment.Invoice = depositInvoice;

                            var rentalContractPayment = contract.ContractPayments.FirstOrDefault(c => c.Type.Equals(ContractPaymentTypeEnum.Rental.ToString()) && c.IsFirstRentalPayment == true);
                            rentalInvoice.Amount += rentalContractPayment.Amount;
                            rentalContractPayment.Invoice = rentalInvoice;

                            context.Contracts.Update(contract);
                        }
                    }

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return (depositInvoice, rentalInvoice);
                }

                return (null, null);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }

        private Invoice InitRentalInvoiceV2(RentingRequest rentingRequest)
        {
            //var totalRentalAmount = rentingRequest.Contracts
            //    .Select(cp => cp.TotalRentPrice)
            //    .Sum();
            //var additionalAmount = rentingRequest.ShippingPrice - rentingRequest.DiscountPrice +
            //                    rentingRequest.TotalServicePrice;
            //totalRentalAmount += additionalAmount;

            var totalRentalAmount = rentingRequest.TotalRentPrice + rentingRequest.ShippingPrice
                    + rentingRequest.TotalServicePrice - rentingRequest.DiscountPrice;

            var invoice = new Invoice
            {
                InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "RENTAL" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                Amount = totalRentalAmount,
                Type = InvoiceTypeEnum.Rental.ToString(),
                Status = InvoiceStatusEnum.Pending.ToString(),
                DateCreate = DateTime.Now,
                AccountPaidId = rentingRequest.AccountOrderId,
            };

            return invoice;
        }

        private Invoice InitDepositInvoiceV2(RentingRequest rentingRequest)
        {
            //var totalDepositAmount = rentingRequest.Contracts
            //    .Select(cp => cp.DepositPrice)
            //    .Sum();

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
    }
}
