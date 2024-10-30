﻿using Common;
using Common.Enum;
using DTOs;
using DTOs.Invoice;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Transactions;

namespace Service.Implement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPayOSService _payOSService;
        private readonly IComponentReplacementTicketRepository _componentReplacementTicketRepository;
        private readonly IContractRepository _contractRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository, 
            IPayOSService payOSService, 
            IComponentReplacementTicketRepository componentReplacementTicketRepository,
            IContractRepository contractRepository)
        {
            _invoiceRepository = invoiceRepository;
            _payOSService = payOSService;
            _componentReplacementTicketRepository = componentReplacementTicketRepository;
            _contractRepository = contractRepository;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAll()
        {
            return await _invoiceRepository.GetAllInvoices();
        }

        public async Task<IEnumerable<InvoiceDto>> GetCustomerInvoice(int customerId)
        {
            var list = await _invoiceRepository.GetAllInvoices();

            return list.Where(i => i.AccountPaidId == customerId).ToList();
        }

        public async Task<object?> GetInvoiceDetail(string invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceDetail(invoiceId);

            if (invoice == null)
            {
                throw new ServiceException(MessageConstant.Invoice.InvoiceNotFound);
            }

            return invoice;
        }

        public async Task<string> GetPaymentUrl(int customerId, string invoiceId, UrlDto urlDto)
        {
            InvoiceDto invoice = await _invoiceRepository.GetInvoice(invoiceId);

            if (invoice == null)
            {
                throw new ServiceException(MessageConstant.Invoice.InvoiceNotFound);
            }

            if (invoice.AccountPaidId != customerId)
            {
                throw new ServiceException(MessageConstant.Invoice.IncorrectAccountIdForInvoice);
            }

            if (invoice.Status == InvoiceStatusEnum.Paid.ToString())
            {
                throw new ServiceException(MessageConstant.Invoice.InvoiceHaveBeenPaid);
            }

            string timestamp = $"{DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds:0}{new Random().Next(1000, 9999)}";
            timestamp = timestamp.Substring(0, Math.Min(10, timestamp.Length));

            invoice.PayOsOrderId = timestamp;

            await _invoiceRepository.UpdateInvoice(invoice);

            string url = await _payOSService.CreatePaymentLink(invoiceId, int.Parse(timestamp), (int)invoice?.Amount, urlDto.UrlCancel, urlDto.UrlReturn);

            return url;
        }

        public async Task<bool> PostTransactionProcess(int customerId, string invoiceId)
        {
            InvoiceDto? invoice = await _invoiceRepository.GetInvoice(invoiceId);

            if (invoice == null)
            {
                throw new ServiceException(MessageConstant.Invoice.InvoiceNotFound);
            }

            if (invoice.AccountPaidId != customerId)
            {
                throw new ServiceException(MessageConstant.Invoice.IncorrectAccountIdForInvoice);
            }

            if (invoice.Status == InvoiceStatusEnum.Paid.ToString())
            {
                throw new ServiceException(MessageConstant.Invoice.InvoiceHaveBeenPaid);
            }

            var transactionReturn = await _payOSService.HandleCodeAfterPaymentQR(invoice?.PayOsOrderId);

            if (transactionReturn == null)
            {

            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    invoice = await _invoiceRepository.AddTransactionToInvoice(transactionReturn, invoiceId);
                    if (invoice == null || invoice.Status != InvoiceStatusEnum.Paid.ToString())
                    {
                        return false;
                    }

                    switch (invoice.Type)
                    {
                        case var type when type.Equals(InvoiceTypeEnum.ComponentTicket.ToString()) && invoice.ComponentReplacementTicketId != null:
                            await _componentReplacementTicketRepository.UpdateTicketStatus(
                                invoice.ComponentReplacementTicketId,
                                ComponentReplacementTicketStatusEnum.Paid.ToString(),
                                customerId
                            );

                            // Handle notification and real-time updates here
                            break;

                        case var type when type.Equals(InvoiceTypeEnum.Deposit.ToString()) || type.Equals(InvoiceTypeEnum.Rental.ToString()):
                            await HandleContractPayment(invoice);
                            break;
                    }

                    scope.Complete();

                    return true;
                }
                catch (Exception ex)
                {
                    {
                        throw new ServiceException(MessageConstant.Invoice.PayInvoiceFail);
                    }
                }
            }
        }
        private async Task HandleContractPayment(InvoiceDto invoice)
        {
            var rentingRequestId = await _contractRepository.UpdateContractPayments(invoice.InvoiceId);

            var isDepositAndFirstRentalPaid = await _contractRepository.IsDepositAndFirstRentalPaid(rentingRequestId);

            if (isDepositAndFirstRentalPaid)
            {
                await _contractRepository.UpdateStatusContractsToSignedInRentingRequest(rentingRequestId, (DateTime)invoice.DatePaid);

                if (invoice.Type.Equals(InvoiceTypeEnum.Rental.ToString()))
                {
                    await _contractRepository.ScheduleNextRentalPayment(rentingRequestId);
                }
            }
        }

        private TransactionReturn GenerateSampleTransactionReturn()
        {
            return new TransactionReturn
            {
                Reference = "TRX" + Guid.NewGuid().ToString(), // Sample reference ID
                AccountNumber = "1234567890", // Sample account number
                AccountName = "Nguyen Van A", // Sample account name
                BankCode = "VCB", // Sample bank code (e.g., Vietcombank)
                BankName = "Vietcombank", // Sample bank name
                Amount = 1500000.50, // Sample transaction amount
                Description = "Payment for renting machinery", // Sample transaction description
                TransactionDate = DateTime.Now // Current date and time for the transaction
            };
        }

    }
}
