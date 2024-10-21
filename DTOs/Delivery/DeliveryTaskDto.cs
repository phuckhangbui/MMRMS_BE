using DTOs.Contract;

namespace DTOs.DeliveryTask
{
    public class DeliveryTaskDto
    {
        public int DeliveryTaskId { get; set; }

        public int? StaffId { get; set; }

        public string? StaffName { get; set; }

        public string? ContractId { get; set; }

        public DateTime? DateShip { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string? Note { get; set; }

        public ContractAddressDto? ContractAddress { get; set; }

    }
}
