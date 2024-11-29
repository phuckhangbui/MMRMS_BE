using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Delivery
{
    public class CreateDeliveryTaskDto
    {
        //[Required(ErrorMessage = MessageConstant.DeliveryTask.DeliveryTaskIdRequired)]
        //public int DeliveryTaskId { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.StaffIdRequired)]
        public int StaffId { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.DateshipIsRequired)]
        public string DateShip { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.DeliveryVehicleCounterRequired)]
        public int DeliveryVehicleCounter { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.ContractIdListRequired)]
        public List<string> ContractIdList { get; set; }

        public string? Note { get; set; }
    }
}
