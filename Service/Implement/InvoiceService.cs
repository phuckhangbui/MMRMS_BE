using Common;
using Common.Enum;
using DTOs;
using DTOs.Invoice;
using DTOs.MachineSerialNumber;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IMachineSerialNumberRepository _machineSerialNumberRepository;
        private readonly IRentingRequestRepository _rentingRequestRepository;
        private readonly IHubContext<ComponentReplacementTicketHub> _componentReplacementTicketHub;
        private readonly INotificationService _notificationService;
        private readonly IMembershipRankService _membershipRankService;

        public InvoiceService(IInvoiceRepository invoiceRepository,
            IPayOSService payOSService,
            IComponentReplacementTicketRepository componentReplacementTicketRepository,
            IContractRepository contractRepository,
            IMachineSerialNumberRepository machineSerialNumberRepository,
            IRentingRequestRepository rentingRequestRepository,
            IHubContext<ComponentReplacementTicketHub> componentReplacementTicketHub,
            INotificationService notificationService,
            IMembershipRankService membershipRankService)
        {
            _invoiceRepository = invoiceRepository;
            _payOSService = payOSService;
            _componentReplacementTicketRepository = componentReplacementTicketRepository;
            _contractRepository = contractRepository;
            _machineSerialNumberRepository = machineSerialNumberRepository;
            _rentingRequestRepository = rentingRequestRepository;
            _componentReplacementTicketHub = componentReplacementTicketHub;
            _notificationService = notificationService;
            _membershipRankService = membershipRankService;
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

                    await _membershipRankService.UpdateMembershipRankForCustomer((int)invoice.AccountPaidId, invoice.Amount ?? 0);

                    switch (invoice.Type)
                    {
                        case var type when type.Equals(InvoiceTypeEnum.ComponentTicket.ToString()) && invoice.ComponentReplacementTicketId != null:
                            await _componentReplacementTicketRepository.UpdateTicketStatus(
                                invoice.ComponentReplacementTicketId,
                                ComponentReplacementTicketStatusEnum.Paid.ToString(),
                                customerId
                            );

                            var ticket = await _componentReplacementTicketRepository.GetTicket(invoice.ComponentReplacementTicketId);

                            //send notification to staff
                            await _notificationService.SendNotificationToStaffWhenCustomerPayTicket(ticket, ticket.ComponentReplacementTicketId);

                            //realtime for ticket
                            await _componentReplacementTicketHub.Clients.All.SendAsync("OnUpdateComponentReplacementTicketStatus", invoice.ComponentReplacementTicketId);

                            break;

                        case var type when type.Equals(InvoiceTypeEnum.Deposit.ToString()) ||
                                            type.Equals(InvoiceTypeEnum.Rental.ToString()):
                            await ProcessContractInvoice(invoice);
                            break;
                    }

                    scope.Complete();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new ServiceException(MessageConstant.Invoice.PayInvoiceFail);
                }
            }
        }

        private async Task ProcessContractInvoice(InvoiceDto invoice)
        {
            var invoiceDetail = await _invoiceRepository.GetInvoiceDetail(invoice.InvoiceId);

            if (invoiceDetail != null && invoiceDetail is ContractInvoiceDto contractInvoice)
            {
                if (!contractInvoice.ContractPayments.IsNullOrEmpty())
                {
                    foreach (var contractPayment in contractInvoice.ContractPayments)
                    {
                        contractPayment.Status = ContractPaymentStatusEnum.Paid.ToString();
                        contractPayment.CustomerPaidDate = invoice.DatePaid;

                        await _contractRepository.UpdateContractPayment(contractPayment);

                        if (contractPayment.Type.Equals(ContractPaymentTypeEnum.Extend.ToString()))
                        {
                            await _contractRepository.UpdateContractStatus(contractPayment.ContractId, ContractStatusEnum.Signed.ToString());
                        }
                    }
                }

                var rentingRequestId = contractInvoice.RentingRequestId;
                var rentingRequest = await _rentingRequestRepository.GetCustomerRentingRequest(rentingRequestId, (int)contractInvoice.AccountPaidId);
                if (rentingRequest != null &&
                    rentingRequest.Status.Equals(RentingRequestStatusEnum.UnPaid.ToString()) &&
                    rentingRequest.PendingInvoices.IsNullOrEmpty())
                {
                    //Both invoice paid
                    foreach (var contractPayment in contractInvoice.ContractPayments)
                    {
                        await _contractRepository.UpdateContractStatus(contractPayment.ContractId, ContractStatusEnum.Signed.ToString());
                    }

                    await _rentingRequestRepository.UpdateRentingRequestStatus(rentingRequestId, RentingRequestStatusEnum.Signed.ToString());

                    if (rentingRequest.IsOnetimePayment == false)
                    {
                        await _invoiceRepository.GenerateMonthlyInvoices(rentingRequestId);
                    }
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

        public async Task<InvoiceDto> CreateRefundInvoice(int accountId, RefundInvoiceRequestDto refundInvoiceRequestDto)
        {
            var contract = await _contractRepository.GetContractById(refundInvoiceRequestDto.ContractId);

            if (contract == null || !contract.Status.Equals(ContractStatusEnum.AwaitingRefundInvoice.ToString()))
            {
                throw new ServiceException(MessageConstant.Contract.ContractNotValidToCreateRefundInvoice);
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var note = GlobalConstant.RefundInvoiceNote + refundInvoiceRequestDto.ContractId;
                var invoice = await _invoiceRepository.CreateInvoice(refundInvoiceRequestDto.Amount ?? 0, InvoiceTypeEnum.Refund.ToString(), accountId, note);

                await _contractRepository.SetInvoiceForContractPayment(contract.ContractId, invoice.InvoiceId, ContractPaymentTypeEnum.Refund.ToString());

                await _contractRepository.UpdateContractStatus(contract.ContractId, ContractStatusEnum.Completed.ToString());
                //MachineSerialNumber
                var machineSerialNumber = await _machineSerialNumberRepository.GetMachineSerialNumber(contract.SerialNumber);
                var machineSerialNumberUpdateDto = new MachineSerialNumberUpdateDto
                {
                    ActualRentPrice = machineSerialNumber.ActualRentPrice ?? 0,
                    Status = machineSerialNumber.Status,
                };

                var componentList = await _machineSerialNumberRepository.GetMachineComponent(machineSerialNumber.SerialNumber);
                var isMachineMaintenance = false;
                if (!componentList.IsNullOrEmpty())
                {
                    isMachineMaintenance = componentList.Any(c => c.Status == MachineSerialNumberComponentStatusEnum.Broken.ToString());
                }
                if (isMachineMaintenance)
                {
                    machineSerialNumberUpdateDto.Status = MachineSerialNumberStatusEnum.Maintenance.ToString();
                }
                else
                {
                    machineSerialNumberUpdateDto.Status = MachineSerialNumberStatusEnum.Available.ToString();
                }

                await _machineSerialNumberRepository.UpdateMachineSerialNumber(machineSerialNumber.SerialNumber, machineSerialNumberUpdateDto, accountId);

                scope.Complete();

                return invoice;
            }
            catch (Exception ex)
            {
                throw new ServiceException(MessageConstant.Invoice.CreateInvoiceFail);
            }
        }
    }
}
