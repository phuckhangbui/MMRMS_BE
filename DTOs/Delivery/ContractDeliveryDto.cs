namespace DTOs.Delivery
{
    public class ContractDeliveryDto
    {
        public int ContractDeliveryId { get; set; }

        public string? ContractId { get; set; }

        public string? SerialNumber { get; set; }

        public int? MachineId { get; set; }

        public string? MachineName { get; set; }

        public string? MachineModel { get; set; }

        public double? Weight { get; set; }

        public int? DeliveryTaskId { get; set; }

        public string? PictureUrl { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }
    }
}
