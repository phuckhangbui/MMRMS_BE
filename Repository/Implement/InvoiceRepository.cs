using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.ContractPayment;
using DTOs.Invoice;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IMapper _mapper;

        public InvoiceRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<InvoiceDto?> AddTransactionToInvoice(TransactionReturn transactionReturn, string invoiceId)
        {
            var invoice = await InvoiceDao.Instance.GetInvoice(invoiceId);

            if (invoice == null)
            {
                throw new Exception(MessageConstant.Invoice.InvoiceNotFound);
            }

            var digitalTransaction = _mapper.Map<DigitalTransaction>(transactionReturn);

            digitalTransaction.InvoiceId = invoiceId;
            digitalTransaction.DigitalTransactionId = transactionReturn.Reference;
            digitalTransaction.PayOsOrderId = invoice.PayOsOrderId;
            await DigitalTransactionDao.Instance.CreateAsync(digitalTransaction);

            invoice.Status = InvoiceStatusEnum.Paid.ToString();
            invoice.PaymentMethod = InvoicePaymentTypeEnum.Digital.ToString();
            invoice.DigitalTransactionId = transactionReturn.Reference;
            invoice.DatePaid = transactionReturn.TransactionDate;

            invoice = await InvoiceDao.Instance.UpdateAsync(invoice);
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoices()
        {
            var invoices = await InvoiceDao.Instance.GetInvoices();

            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        public async Task<InvoiceDto> GetInvoice(string invoiceId)
        {
            var invoice = await InvoiceDao.Instance.GetInvoice(invoiceId);

            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<object?> GetInvoiceDetail(string invoiceId)
        {
            var invoice = await InvoiceDao.Instance.GetInvoice(invoiceId);

            if (invoice != null)
            {
                if (invoice.Type!.Equals(InvoiceTypeEnum.Rental.ToString()) || invoice.Type.Equals(InvoiceTypeEnum.Deposit.ToString()))
                {
                    var contractInvoice = _mapper.Map<ContractInvoiceDto>(invoice);
                    var contractPayments = await ContractPaymentDao.Instance.GetContractPaymentsByInvoiceId(invoiceId);
                    if (!contractPayments.IsNullOrEmpty())
                    {
                        contractInvoice.ContractPayments = _mapper.Map<List<ContractPaymentDto>>(contractPayments);

                        var contractPayment = contractPayments.FirstOrDefault(cp => cp.InvoiceId.Equals(invoiceId));
                        var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(contractPayment.Contract.RentingRequestId);
                        contractInvoice.RentingRequestId = rentingRequest?.RentingRequestId;

                        var firstRentalContractPayment = contractPayments.FirstOrDefault(cp => (bool)cp.IsFirstRentalPayment);
                        if (firstRentalContractPayment != null)
                        {
                            if (firstRentalContractPayment != null)
                            {
                                var firstRentalPayment = new FirstRentalPaymentDto()
                                {
                                    DiscountPrice = rentingRequest.DiscountPrice,
                                    ShippingPrice = rentingRequest.ShippingPrice,
                                    TotalServicePrice = rentingRequest.TotalServicePrice,
                                };

                                var firstRentalInvoice = contractInvoice.ContractPayments.Find(i => i.ContractPaymentId == firstRentalContractPayment.ContractPaymentId);
                                firstRentalInvoice.FirstRentalPayment = firstRentalPayment;
                            }
                        }
                    }

                    return contractInvoice;
                }
                else
                {
                    return _mapper.Map<InvoiceDto>(invoice);
                }
            }

            return null;
        }

        public async Task UpdateInvoice(InvoiceDto invoiceDto)
        {
            var invoice = _mapper.Map<Invoice>(invoiceDto);

            await InvoiceDao.Instance.UpdateAsync(invoice);
        }

        public async Task<InvoiceDto> CreateInvoice(double amount, string type, int accountPaidId)
        {
            var invoice = new Invoice
            {
                InvoiceId = await GenerateInvoiceId(),
                Amount = amount,
                Type = type,
                Status = InvoiceStatusEnum.Pending.ToString(),
                DateCreate = DateTime.Now,
                AccountPaidId = accountPaidId,
            };

            if (type.Equals(InvoiceTypeEnum.Refund.ToString()))
            {
                invoice.Status = InvoiceStatusEnum.Paid.ToString();
                invoice.DatePaid = DateTime.Now;
            }

            invoice = await InvoiceDao.Instance.CreateAsync(invoice);

            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task UpdateInvoiceStatus(string invoiceId, string status)
        {
            var invoice = await InvoiceDao.Instance.GetInvoice(invoiceId);

            if (invoice == null)
            {
                throw new Exception(MessageConstant.Invoice.InvoiceNotFound);
            }

            invoice.Status = status;

            await InvoiceDao.Instance.UpdateAsync(invoice);
        }

        private async Task<string> GenerateInvoiceId()
        {
            int currentTotalInvoices = await InvoiceDao.Instance.GetTotalInvoiceByDate(DateTime.Now);
            string datePart = DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern);
            string sequencePart = (currentTotalInvoices + 1).ToString("D4");
            return $"{GlobalConstant.InvoiceIdPrefixPattern}{datePart}{GlobalConstant.SequenceSeparator}{sequencePart}";
        }

        public async Task CreateInvoice(string rentingRequestId)
        {
            var currentRequest = await RentingRequestDao.Instance.GetRentingRequest(rentingRequestId);

            if (currentRequest != null)
            {
                var totalDepositAmount = currentRequest.Contracts.Select(c => c.DepositPrice).Sum() ?? 0;
                var depositInvoice = await CreateInvoice(totalDepositAmount, InvoiceTypeEnum.Deposit.ToString(), (int)currentRequest.AccountOrderId);
                foreach (var payment in currentRequest.Contracts.SelectMany(c => c.ContractPayments)
                                                                  .Where(cp => cp.Type == ContractPaymentTypeEnum.Deposit.ToString()))
                {
                    payment.InvoiceId = depositInvoice.InvoiceId;
                    await ContractPaymentDao.Instance.UpdateAsync(payment);
                }

                //
                if (currentRequest.IsOnetimePayment == true)
                {
                    var totalRentAmount = currentRequest.Contracts.Select(c => c.TotalRentPrice).Sum() ?? 0;
                    totalRentAmount = totalRentAmount + currentRequest.TotalServicePrice + currentRequest.ShippingPrice - currentRequest.DiscountPrice ?? 0;
                    var rentalInvoice = await CreateInvoice(totalRentAmount, InvoiceTypeEnum.Rental.ToString(), (int)currentRequest.AccountOrderId);
                    foreach (var payment in currentRequest.Contracts.SelectMany(c => c.ContractPayments)
                                                                          .Where(cp => cp.Type == ContractPaymentTypeEnum.Rental.ToString()))
                    {
                        payment.InvoiceId = rentalInvoice.InvoiceId;
                        await ContractPaymentDao.Instance.UpdateAsync(payment);
                    }
                }
                else
                {
                    var firstMonthTotalAmount = currentRequest.Contracts
                                .SelectMany(c => c.ContractPayments)
                                .Where(cp => cp.Type == ContractPaymentTypeEnum.Rental.ToString() && cp.IsFirstRentalPayment == true)
                                .Sum(cp => cp.Amount ?? 0);
                    firstMonthTotalAmount = firstMonthTotalAmount + currentRequest.TotalServicePrice + currentRequest.ShippingPrice - currentRequest.DiscountPrice ?? 0;

                    var rentalInvoice = await CreateInvoice(firstMonthTotalAmount, InvoiceTypeEnum.Rental.ToString(), (int)currentRequest.AccountOrderId);
                    foreach (var payment in currentRequest.Contracts.SelectMany(c => c.ContractPayments)
                                                                          .Where(cp => cp.Type == ContractPaymentTypeEnum.Rental.ToString() && cp.IsFirstRentalPayment == true))
                    {
                        payment.InvoiceId = rentalInvoice.InvoiceId;
                        await ContractPaymentDao.Instance.UpdateAsync(payment);
                    }
                }
            }
        }

        public async Task GenerateMonthlyInvoices(string rentingRequestId)
        {
            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequest(rentingRequestId);

            var paymentsGroupedByMonth = rentingRequest.Contracts
                .SelectMany(contract => contract.ContractPayments)
                .Where(payment => payment.Type == ContractPaymentTypeEnum.Rental.ToString() && payment.Status == ContractPaymentStatusEnum.Pending.ToString())
                .GroupBy(payment => new { payment.DateFrom.Value.Year, payment.DateFrom.Value.Month })
                .OrderBy(group => group.Key.Year).ThenBy(group => group.Key.Month);

            foreach (var monthlyGroup in paymentsGroupedByMonth)
            {
                double totalAmount = monthlyGroup.Sum(payment => payment.Amount ?? 0);

                var rentalInvoice = await CreateInvoice(totalAmount, InvoiceTypeEnum.Rental.ToString(), (int)rentingRequest.AccountOrderId);

                foreach (var payment in monthlyGroup)
                {
                    payment.InvoiceId = rentalInvoice.InvoiceId;
                    await ContractPaymentDao.Instance.UpdateAsync(payment);
                }
            }
        }
    }
}
