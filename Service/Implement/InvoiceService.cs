﻿using Common;
using Common.Enum;
using DTOs;
using DTOs.Invoice;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPayOSService _payOSService;

        public InvoiceService(IInvoiceRepository invoiceRepository, IPayOSService payOSService)
        {
            _invoiceRepository = invoiceRepository;
            _payOSService = payOSService;
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

            var transactionReturn = await _payOSService.HandleCodeAfterPaymentQR(invoice?.PayOsOrderId);

            if (transactionReturn == null)
            {

            }

            invoice = await _invoiceRepository.AddTransactionToInvoice(transactionReturn, invoiceId);
            if (invoice.Status == InvoiceStatusEnum.Paid.ToString())
            {
                return true;
            }

            return false;
        }

        private TransactionReturn GenerateSampleTransactionReturn()
        {
            return new TransactionReturn
            {
                Reference = "TRX123456789", // Sample reference ID
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
