using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.RentingRequest
{
    public class RentingRequestReviewDto
    {
        [Required]
        public int? NumOfMonth { get; set; }

        public List<RentingRequestReviewSerialNumberDto>? RentingRequestReviewSerialNumbers { get; set; }
    }

    public class RentingRequestReviewSerialNumberDto
    {
        [Required]
        public string? SerialNumber { get; set; }

        [Required]
        public double? RentPricePerDays { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.DateStartRequired)]
        //[FutureOrPresentDate(ErrorMessage = MessageConstant.RentingRequest.DateFutureOrPresent)]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.DateEndRequired)]
        //[FutureOrPresentDate(ErrorMessage = MessageConstant.RentingRequest.DateFutureOrPresent)]
        public DateTime DateEnd { get; set; }
    }

    public class RentingRequestReviewResponseDto
    {
        public int Time { get; set; }
        public List<RentingRequestReviewResponseSerialNumberDto> RentingRequestReviewResponseSerialNumbers { get; set; }
    }

    public class RentingRequestReviewResponseSerialNumberDto
    {
        public string? SerialNumber { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int? RentPeriod { get; set; }
        public double? RentPrice { get; set; }
        public double? TotalRentPrice { get; set; }
    }
}
