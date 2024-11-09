using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.RentingRequest
{
    public class RentingRequestDto
    {
        public string? RentingRequestId { get; set; }
        public int? AccountOrderId { get; set; }
        public string? AccountOrderName { get; set; }
        public DateTime? DateCreate { get; set; }
        public double? TotalRentPrice { get; set; }
        public double? TotalDepositPrice { get; set; }
        public double? TotalServicePrice { get; set; }
        public double? ShippingPrice { get; set; }
        public double? DiscountPrice { get; set; }
        public double? TotalAmount { get; set; }
        public bool? IsOnetimePayment { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
    }

    public class CustomerRentingRequestDto : RentingRequestDto
    {
        public List<PendingInvoiceDto> PendingInvoices { get; set; } = new List<PendingInvoiceDto>();
    }

    public class PendingInvoiceDto
    {
        public string? InvoiceId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
    }

    public class NewRentingRequestDto
    {
        [Required(ErrorMessage = MessageConstant.RentingRequest.AddressIdRequired)]
        public int AddressId { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.ShippingPriceRequired)]
        public double ShippingPrice { get; set; }

        public double DiscountPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.IsOnetimePaymentRequired)]
        public bool IsOnetimePayment { get; set; }

        public string? Note { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.RequestMachinesRequired)]
        public List<RentingRequestSerialNumberDto> RentingRequestSerialNumbers { get; set; } = new List<RentingRequestSerialNumberDto>();

        [Required(ErrorMessage = MessageConstant.RentingRequest.ServiceRentingRequestsRequired)]
        public List<int> ServiceRentingRequests { get; set; } = new List<int>();
    }

    public class RentingRequestSerialNumberDto
    {
        [Required(ErrorMessage = MessageConstant.RentingRequest.MachineIdRequired)]
        public int MachineId { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.SerialRequired)]
        public string? SerialNumber { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.DateStartRequired)]
        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.DateEndRequired)]
        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }
    }
}
