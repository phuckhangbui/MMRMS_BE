using DTOs.AccountAddressDto;
using DTOs.ContractPayment;
using DTOs.RentingRequest;
using DTOs.Term;

namespace DTOs.Contract
{
    public class ContractDetailDto : ContractDto
    {
        public bool IsOnetimePayment { get; set; }
        public AccountOrderDto AccountOrder { get; set; }
        public string? Content { get; set; }
        public int? AccountSignId { get; set; }
        public string? RentingRequestId { get; set; }
        public double? RentPrice { get; set; }
        public double? DepositPrice { get; set; }
        public int? NumberOfMonth { get; set; }
        public int? RentPeriod { get; set; }
        public double? TotalRentPrice { get; set; }
        public AccountBusinessDto AccountBusiness { get; set; }
        public List<ContractPaymentDto> ContractPayments { get; set; }
        public List<ContractTermDto> ContractTerms { get; set; }
    }
}
