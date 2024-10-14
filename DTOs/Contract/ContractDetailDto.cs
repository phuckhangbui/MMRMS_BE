using DTOs.AccountAddressDto;
using DTOs.RentingRequest;
using DTOs.Term;

namespace DTOs.Contract
{
    public class ContractDetailDto : ContractDto
    {
        public List<ContractTermDto> ContractTerms { get; set; }
        public bool IsOnetimePayment { get; set; }
        //public List<ServiceRentingRequestDto> ServiceRentingRequests { get; set; }
        //public List<ContractProductDetailDto> ContractProductDetails { get; set; }
        public AccountOrderDto AccountOrder { get; set; }
        public string? SerialNumber { get; set; }
        public string? Content { get; set; }
        public int? AccountSignId { get; set; }
        public int? ContractAddressId { get; set; }
        public string? RentingRequestId { get; set; }
        public double? RentPrice { get; set; }
        public double? DepositPrice { get; set; }
        public int? NumberOfMonth { get; set; }
        public double? TotalRentPrice { get; set; }
        //public AddressDto Address { get; set; }
        public AccountBusinessDto AccountBusiness { get; set; }
    }

    public class ContractProductDetailDto : RentingRequestProductDetailDto
    {
        public List<ContractSerialNumberProductDto> ContractSerialNumberProducts { get; set; } = new List<ContractSerialNumberProductDto>();
    }

    public class ContractSerialNumberProductDto
    {
        public string SerialNumber { get; set; }
        public double DepositPrice { get; set; }
        public double RentPrice { get; set; }
    }
}
