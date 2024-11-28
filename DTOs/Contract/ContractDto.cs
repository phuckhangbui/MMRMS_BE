namespace DTOs.Contract
{
    public class ContractDto
    {
        public string? ContractId { get; set; }
        public string? ContractName { get; set; }
        public string? RentingRequestId { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateSign { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? Status { get; set; }
        public double? DepositPrice { get; set; }
        public int? RentPeriod { get; set; }
        public double? TotalRentPrice { get; set; }
        public int? MachineId { get; set; }
        public string? MachineName { get; set; }
        public double? Weight { get; set; }
        public string? SerialNumber { get; set; }
        public double? RentPrice { get; set; }
        public string? Thumbnail { get; set; }
        public int? AccountSignId { get; set; }
        public string? BaseContractId { get; set; }
        public bool? IsExtended { get; set; }
        public ContractAddressDto? ContractAddress { get; set; }
    }
}
