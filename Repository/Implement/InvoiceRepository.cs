﻿using AutoMapper;
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


            invoice.Status = InvoiceStatusEnum.Paid.ToString();
            invoice.DigitalTransactionId = transactionReturn.Reference;
            invoice.DigitalTransaction = digitalTransaction;

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
    }
}
