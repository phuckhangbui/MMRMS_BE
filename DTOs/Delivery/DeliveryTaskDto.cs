using DTOs.Contract;

namespace DTOs.Delivery
{
    public class DeliveryTaskDto
    {
        public int DeliveryTaskId { get; set; }

        public string? SerialNumber { get; set; }

        public int? StaffId { get; set; }

        public string? StaffName { get; set; }

        public int? ManagerId { get; set; }

        public string? ManagerName { get; set; }

        public int? CustomerId { get; set; }

        public DateTime? DateShip { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }

        public string? ConfirmationPictureUrl { get; set; }

        public string? ReceiverName { get; set; }

        public string? Type { get; set; }



        public ContractAddressDto? ContractAddress { get; set; }

    }
}
