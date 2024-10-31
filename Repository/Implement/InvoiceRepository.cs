using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.ContractPayment;
using DTOs.Invoice;
using DTOs.RentingRequest;
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

                        var firstRentalContractPayment = contractPayments.FirstOrDefault(cp => (bool)cp.IsFirstRentalPayment);
                        if (firstRentalContractPayment != null)
                        {
                            var rentingRequest = await RentingRequestDao.Instance.GetRentingRequestById(firstRentalContractPayment.Contract.RentingRequestId);

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

        public async Task<(InvoiceDto DepositInvoice, InvoiceDto RentalInvoice)> InitInvoices(RentingRequestDto rentingRequest)
        {
            var depositInvoice = new Invoice
            {
                InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "DEPOSIT" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                Amount = 0,
                Type = InvoiceTypeEnum.Deposit.ToString(),
                Status = InvoiceStatusEnum.Pending.ToString(),
                DateCreate = DateTime.Now,
                AccountPaidId = rentingRequest.AccountOrderId,
            };

            var rentalInvoice = new Invoice
            {
                InvoiceId = GlobalConstant.InvoiceIdPrefixPattern + "RENTAL" + DateTime.Now.ToString(GlobalConstant.DateTimeFormatPattern),
                Amount = 0,
                Type = InvoiceTypeEnum.Rental.ToString(),
                Status = InvoiceStatusEnum.Pending.ToString(),
                DateCreate = DateTime.Now,
                AccountPaidId = rentingRequest.AccountOrderId,
            };

            await InvoiceDao.Instance.CreateAsync(depositInvoice);
            await InvoiceDao.Instance.CreateAsync(rentalInvoice);

            return (
                _mapper.Map<InvoiceDto>(depositInvoice),
                _mapper.Map<InvoiceDto>(rentalInvoice)
            );
        }
    }
}
