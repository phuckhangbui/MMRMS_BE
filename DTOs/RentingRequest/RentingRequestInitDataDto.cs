using Common;
using DTOs.Machine;
using DTOs.MachineSerialNumber;
using DTOs.MembershipRank;
using DTOs.RentingService;
using DTOs.Term;
using DTOs.Validation;
using System.ComponentModel.DataAnnotations;

namespace DTOs.RentingRequest
{
    public class RentingRequestInitDataDto
    {
        public MembershipRankDto MembershipRank { get; set; }
        public List<RentingRequestMachineDataDto> RentingRequestMachineDatas { get; set; }
        public List<RentingServiceDto> RentingServices { get; set; }
        public List<TermDto> Terms { get; set; }
    }

    public class RentingRequestMachineDataDto
    {
        public int MachineId { get; set; }
        public string? MachineName { get; set; }
        public double MachinePrice { get; set; }
        public double RentPrice { get; set; }
        public int Quantity { get; set; }
        public string? CategoryName { get; set; }
        public string? ThumbnailUrl { get; set; }
        public double ShipPricePerKm { get; set; }
        public List<double> RentPrices { get; set; }
        public List<MachineTermDto> MachineTerms { get; set; } = new List<MachineTermDto>();
        public List<MachineSerialNumberDto> MachineSerialNumbers { get; set; } = new List<MachineSerialNumberDto>();
    }

    public class RentingRequestMachineInRangeDto
    {
        [Required(ErrorMessage = MessageConstant.RentingRequest.RequestMachinesRequired)]
        public List<int> MachineIds { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.DateStartRequired)]
        [FutureOrPresentDate(ErrorMessage = MessageConstant.RentingRequest.DateFutureOrPresent)]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingRequest.DateEndRequired)]
        [FutureOrPresentDate(ErrorMessage = MessageConstant.RentingRequest.DateFutureOrPresent)]
        public DateTime DateEnd { get; set; }
    }
}
