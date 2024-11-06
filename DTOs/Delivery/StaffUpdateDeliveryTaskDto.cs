using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Delivery
{
    public class StaffUpdateDeliveryTaskDto
    {
        [Required(ErrorMessage = MessageConstant.DeliveryTask.DeliveryTaskIdRequired)]
        public int DeliveryTaskId { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.ReceiverNameRequired)]
        public string ReceiverName { get; set; }

        public string? Note { get; set; }

        public string? ConfirmationPictureUrl { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.ContractDeliveryListRequired)]
        public IEnumerable<StaffUpdateContractDeliveryDto> ContractDeliveries { get; set; }
    }


    public class StaffUpdateContractDeliveryDto
    {
        [Required(ErrorMessage = MessageConstant.DeliveryTask.DeliveryTaskIdRequired)]
        public int ContractDeliveryId { get; set; }

        public string? Note { get; set; }

        public string? PictureUrl { get; set; }

    }

    public class StaffFailDeliveryTaskDto
    {
        [Required(ErrorMessage = MessageConstant.DeliveryTask.DeliveryTaskIdRequired)]
        public int DeliveryTaskId { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.NoteRequired)]
        public string Note { get; set; }
    }
}
