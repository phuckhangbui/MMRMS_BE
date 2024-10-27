using DTOs.Machine;
using DTOs.MembershipRank;
using DTOs.RentingService;
using DTOs.Term;

namespace DTOs.RentingRequest
{
    public class RentingRequestInitDataDto
    {
        //public List<AccountPromotionDto> AccountPromotions { get; set; }
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
        public string CategoryName { get; set; }
        public string ThumbnailUrl { get; set; }
        public double ShipPricePerKm { get; set; }
        public List<double> RentPrices { get; set; }
        public List<MachineTermDto> MachineTerms { get; set; }
    }

    public class RentingRequestMachineInRangeDto
    {
        public List<int> MachineIds { get; set; }
        public int NumberOfMonth { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
