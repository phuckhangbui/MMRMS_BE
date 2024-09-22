using DTOs.Term;

namespace DTOs.Contract
{
    public class ContractDto
    {
        public string ContractId { get; set; } = null!;

        public string? ContractName { get; set; }

        public int? AccountSignId { get; set; }

        public int? AddressId { get; set; }

        public string? HiringRequestId { get; set; }

        public double? TotalRentPrice { get; set; }

        public double? ShippingPrice { get; set; }

        public double? TotalDepositPrice { get; set; }

        public double? DiscountPrice { get; set; }

        public double? FinalAmount { get; set; }

        public string? Content { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateSign { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string? Status { get; set; }
    }

    public class ContractRequestDto
    {
        public string ContractName { get; set; }

        public int AccountSignId { get; set; }

        public int AddressId { get; set; }

        public string HiringRequestId { get; set; }

        public string Content { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public List<ContractTermRequestDto> ContractTerms { get; set; }
    }
}
