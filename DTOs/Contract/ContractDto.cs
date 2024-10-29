using Common;
using DTOs.MachineSerialNumber;
using DTOs.Term;
using DTOs.Validation;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Contract
{
    public class ContractDto
    {
        public string ContractId { get; set; } = null!;
        public string? ContractName { get; set; }
        public string? RentingRequestId { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateSign { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? Status { get; set; }
        public int? MachineId { get; set; }
        public string? MachineName { get; set; }
        public string? SerialNumber { get; set; }
        public double? RentPrice { get; set; }
        public string? Thumbnail { get; set; }
        public int? AccountSignId { get; set; }

    }

    public class ContractRequestDto
    {
        [Required(ErrorMessage = MessageConstant.Contract.ContractNameRequired)]
        public string ContractName { get; set; }

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

        [Required(ErrorMessage = MessageConstant.Contract.MachineSerialNumbersRequired)]
        public List<MachineSerialNumberRentRequestDto> MachineSerialNumbers { get; set; }
    }
}
