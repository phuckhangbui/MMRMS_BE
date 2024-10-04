using Common;
using DTOs.AccountAddressDto;
using DTOs.SerialNumberProduct;
using DTOs.Term;
using DTOs.Validation;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Contract
{
    public class ContractDto
    {
        public string ContractId { get; set; } = null!;

        public string? ContractName { get; set; }

        public int? AccountSignId { get; set; }

        public int? ContractAddressId { get; set; }

        public string? RentingRequestId { get; set; }

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

        public List<AccountBusinessDto> AccountBusinesses { get; set; }

        public string Name { get; set; }
    }

    public class ContractDetailDto : ContractDto
    {
        public List<ContractTermDto> ContractTerms { get; set; }
    }

    public class ContractRequestDto
    {
        [Required(ErrorMessage = MessageConstant.Contract.ContractNameRequired)]
        public string ContractName { get; set; }

        //[Required(ErrorMessage = MessageConstant.Contract.AccountSignIdRequired)]
        //public int AccountSignId { get; set; }

        //[Required(ErrorMessage = MessageConstant.Contract.AddressIdRequired)]
        //public int AddressId { get; set; }

        [Required(ErrorMessage = MessageConstant.Contract.RentingRequestIdRequired)]
        public string RentingRequestId { get; set; }

        [Required(ErrorMessage = MessageConstant.Contract.ContentRequired)]
        public string Content { get; set; }

        [Required(ErrorMessage = MessageConstant.Contract.DateStartRequired)]
        [FutureOrPresentDate(ErrorMessage = MessageConstant.Contract.DateStartFutureOrPresent)]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = MessageConstant.Contract.DateEndRequired)]
        public DateTime DateEnd { get; set; }

        [Required(ErrorMessage = MessageConstant.Contract.ContractTermsRequired)]
        public List<ContractTermRequestDto> ContractTerms { get; set; }

        [Required(ErrorMessage = MessageConstant.Contract.SerialNumberProductsRequired)]
        public List<SerialNumberProductRentRequestDto> SerialNumberProducts { get; set; }
    }
}
