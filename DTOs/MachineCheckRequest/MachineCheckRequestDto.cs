﻿using DTOs.Contract;

namespace DTOs.MachineCheckRequest
{
    public class MachineCheckRequestDto
    {
        public string MachineCheckRequestId { get; set; }

        public int? MachineTaskId { get; set; }

        public string? ContractId { get; set; }

        public string? SerialNumber { get; set; }

        public int? MachineId { get; set; }

        public string? MachineName { get; set; }

        public string? Thumbnail { get; set; }

        public int? CustomerId { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        public DateTime? DateCreate { get; set; }

        public ContractAddressDto? ContractAddress { get; set; }
    }
}
