using DTOs.AccountAddressDto;
using DTOs.Contract;
using DTOs.RentingRequestAddress;

namespace DTOs.RentingRequest
{
    public class RentingRequestDetailDto
    {
        public string? RentingRequestId { get; set; }
        public int? AccountOrderId { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateStart { get; set; }
        public double? TotalRentPrice { get; set; }
        public double? TotalDepositPrice { get; set; }
        public double? TotalServicePrice { get; set; }
        public double? ShippingPrice { get; set; }
        public double? DiscountPrice { get; set; }
        public int? NumberOfMonth { get; set; }
        public double? TotalAmount { get; set; }
        public bool? IsOnetimePayment { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
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
