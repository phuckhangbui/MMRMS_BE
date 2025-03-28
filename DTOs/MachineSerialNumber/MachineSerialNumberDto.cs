﻿namespace DTOs.MachineSerialNumber
{
    public class MachineSerialNumberDto
    {
        public string SerialNumber { get; set; } = null!;

        public double? ActualRentPrice { get; set; }

        public int? MachineConditionPercent { get; set; }

        public int? MachineId { get; set; }

        public string? MachineName { get; set; }

        public string? MachineModel { get; set; }

        public string? Status { get; set; }

        public DateTime? DateCreate { get; set; }

        public int? RentDaysCounter { get; set; }

        public DateTime? ExpectedAvailableDate { get; set; }

        public DateTime? RentingStartDate { get; set; }

        public DateTime? RentingEndDate { get; set; }
    }
}
