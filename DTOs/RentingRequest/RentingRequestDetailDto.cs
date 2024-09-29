namespace DTOs.RentingRequest
{
    public class RentingRequestDetailDto
    {
        public string RentingRequestId { get; set; }
        public int? AccountOrderId { get; set; }
        public int? AddressId { get; set; }
        public string? ContractId { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateStart { get; set; }
        public double? TotalRentPricePerMonth { get; set; }
        public double? TotalDepositPrice { get; set; }
        public double? ShippingPrice { get; set; }
        public double? DiscountShip { get; set; }
        public double? DiscountPrice { get; set; }
        public int? NumberOfMonth { get; set; }
        public double? TotalAmount { get; set; }
        public bool? IsOnetimePayment { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public List<RentingRequestProductDetailDto> RentingRequestProductDetails { get; set; }
        public List<ServiceRentingRequestDto> ServiceRentingRequests { get; set; }
        public AccountOrderDto AccountOrder { get; set; }
    }

    public class RentingRequestProductDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    public class ServiceRentingRequestDto
    {
        public int ServiceRentingRequestId { get; set; }
        public int RentingServiceId { get; set; }
        public double ServicePrice { get; set; }
        public double DiscountPrice { get; set; }
        public double FinalPrice { get; set; }
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
