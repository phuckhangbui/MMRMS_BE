using DTOs.Contract;

namespace DTOs.MachineCheckRequest
{
    public class MachineCheckRequestDto
    {
        public string RequestId { get; set; }

        public string? ContractId { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        public DateTime? DateCreate { get; set; }

        public ContractAddressDto? ContractAddress { get; set; }
    }
}
