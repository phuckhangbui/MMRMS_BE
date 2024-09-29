namespace DTOs.RentingRequest
{
    public class RentingRequestDto
    {
        public string RentingRequestId { get; set; } = null!;

        public int? AccountOrderId { get; set; }

        public string? AccountOrderName { get; set; }

        public int? AddressId { get; set; }

        public string? ContractId { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateStart { get; set; }

        public int? NumberOfMonth { get; set; }

        public bool? IsOnetimePayment { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }
    }

    public class NewRentingRequestDto
    {
        public int AccountOrderId { get; set; }

        public int AddressId { get; set; }

        public DateTime DateStart { get; set; }

        public double TotalRentPricePerMonth { get; set; }

        public double TotalDepositPrice { get; set; }

        public double ShippingPrice { get; set; }

        public double DiscountShip { get; set; }

        public double DiscountPrice { get; set; }

        public int NumberOfMonth { get; set; }

        public double TotalAmount { get; set; }

        public bool IsOnetimePayment { get; set; }

        public string Note { get; set; }

        public List<NewRentingRequestProductDetailDto> RentingRequestProductDetails { get; set; }

        public List<int> ServiceRentingRequests { get; set; }
    }

    public class NewRentingRequestProductDetailDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
