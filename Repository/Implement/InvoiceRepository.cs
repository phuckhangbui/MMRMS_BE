﻿using AutoMapper;
using BusinessObject;
using Common;
using Common.Enum;
using DAO;
using DTOs.Invoice;
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

        public async Task AddTransactionToInvoice(TransactionReturn transactionReturn, string invoiceId)
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
            await InvoiceDao.Instance.UpdateAsync(invoice);
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

        public async Task UpdateInvoice(InvoiceDto invoiceDto)
        {
            var invoice = _mapper.Map<Invoice>(invoiceDto);

            await InvoiceDao.Instance.UpdateAsync(invoice);
        }
    }
}
