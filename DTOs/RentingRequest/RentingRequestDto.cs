using Common;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = MessageConstant.RentingRequest.AddressIdRequired)]
        public int AddressId { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.DateStartRequired)]
        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.ShippingPriceRequired)]
        public double ShippingPrice { get; set; }

        public double DiscountShip { get; set; }

        public double DiscountPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.NumberOfMonthRequired)]
        public int NumberOfMonth { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.IsOnetimePaymentRequired)]
        public bool IsOnetimePayment { get; set; }

        public string? Note { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.RequestProductsRequired)]
        public List<NewRentingRequestProductDetailDto> RentingRequestProductDetails { get; set; } = new List<NewRentingRequestProductDetailDto>();

        [Required(ErrorMessage = MessageConstant.RentingRequest.ServiceRentingRequestsRequired)]
        public List<int> ServiceRentingRequests { get; set; } = new List<int>();

        public int AccountPromotionId { get; set; }
    }

    public class NewRentingRequestProductDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
