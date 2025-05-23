﻿using DTOs.AccountBusiness;
using DTOs.Contract;
using DTOs.RentingRequestAddress;

namespace DTOs.RentingRequest
{
    public class RentingRequestDetailDto : RentingRequestDto
    {
        public List<ServiceRentingRequestDto> ServiceRentingRequests { get; set; }
        public AccountOrderDto AccountOrder { get; set; }
        public RentingRequestAddressDto RentingRequestAddress { get; set; }
        public AccountBusinessDto AccountBusiness { get; set; }
        public List<ContractDto> Contracts { get; set; }
    }

    public class ServiceRentingRequestDto
    {
        public int RentingServiceId { get; set; }
        public double ServicePrice { get; set; }
        public string RentingServiceName { get; set; }
    }

    public class AccountOrderDto
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
