﻿using DTOs.Account;
using DTOs.AccountBusiness;
using DTOs.ComponentReplacementTicket;
using DTOs.ContractPayment;
using DTOs.ContractTerm;
using DTOs.RentingRequest;

namespace DTOs.Contract
{
    public class ContractDetailDto : ContractDto
    {
        public bool IsOnetimePayment { get; set; }
        public AccountOrderDto? AccountOrder { get; set; }
        public string? Content { get; set; }
        public AccountBusinessDto? AccountBusiness { get; set; }
        public List<ContractPaymentDto>? ContractPayments { get; set; }
        public List<ContractTermDto>? ContractTerms { get; set; }
        public BankAccountRefundDto? BankAccountRefund { get; set; }
        public double? ProvisionalShippingPrice { get; set; }
        public double? ShippingDistance { get; set; }
        public double? ServicePrice { get; set; }
        public List<ComponentReplacementTicketDto>? ComponentReplacementTickets { get; set; }
        public List<ServiceRentingRequestDto>? ContractServices { get; set; }
    }
}
