using DTOs.Contract;

namespace DTOs.MaintenanceRequest
{
    public class MaintenanceRequestDto
    {
        public int RequestId { get; set; }

        public string? ContractId { get; set; }

        public string? SerialNumber { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        public DateTime? DateCreate { get; set; }

        public ContractAddressDto? ContractAddress { get; set; }
    }
}
