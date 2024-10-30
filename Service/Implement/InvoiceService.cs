using Common;
using Common.Enum;
using DTOs;
using DTOs.Invoice;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.SignalRHub;
using System.Transactions;

namespace Service.Implement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPayOSService _payOSService;
        private readonly IComponentReplacementTicketRepository _componentReplacementTicketRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IHubContext<ComponentReplacementTicketHub> _componentReplacementTicketHub;
        private readonly INotificationService _notificationService;


        public InvoiceService(IInvoiceRepository invoiceRepository,
            IPayOSService payOSService,
            IComponentReplacementTicketRepository componentReplacementTicketRepository,
            IContractRepository contractRepository,
            IHubContext<ComponentReplacementTicketHub> componentReplacementTicketHub,
            INotificationService notificationService)
        {
            _invoiceRepository = invoiceRepository;
            _payOSService = payOSService;
            _componentReplacementTicketRepository = componentReplacementTicketRepository;
            _contractRepository = contractRepository;
            _componentReplacementTicketHub = componentReplacementTicketHub;
            _notificationService = notificationService;
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
                    //switch case to process contract and transaction here
                    if (invoice.Type.Equals(InvoiceTypeEnum.ComponentTicket.ToString()) && invoice.ComponentReplacementTicketId != null)
                    {
                        await _componentReplacementTicketRepository.UpdateTicketStatus(invoice.ComponentReplacementTicketId, ComponentReplacementTicketStatusEnum.Paid.ToString(), customerId);

                        var ticket = await _componentReplacementTicketRepository.GetTicket(invoice.ComponentReplacementTicketId);

                        //send notification to staff
                        await _notificationService.SendNotificationToStaffWhenCustomerPayTicket(ticket);

                        //realtime for ticket
                        await _componentReplacementTicketHub.Clients.All.SendAsync("OnUpdateComponentReplacementTicketStatus", invoice.ComponentReplacementTicketId);
                    }
                    else if (invoice.Type.Equals(InvoiceTypeEnum.Deposit.ToString()))
                    {
                        await _contractRepository.UpdateDepositContractPayment(invoiceId);
                    }
                    else if (invoice.Type.Equals(InvoiceTypeEnum.Rental.ToString()))
                    {
                        await _contractRepository.UpdateRentalContractPayment(invoiceId);
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

        //private async Task UpdateComponentTicketInvoice(string componentTicketId, int accountId)
        //{
        //    //var ticket = await _componentReplacementTicketRepository.GetTicket(componentTicketId);


        //}

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
