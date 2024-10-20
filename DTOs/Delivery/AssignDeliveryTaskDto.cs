using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DeliveryTask
{
    public class AssignDeliveryTaskDto
    {
        [Required(ErrorMessage = MessageConstant.DeliveryTask.DeliveryTaskIdRequired)]
        public int DeliveryTaskId { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.StaffIdRequired)]
        public int StaffId { get; set; }

        [Required(ErrorMessage = MessageConstant.DeliveryTask.DateshipIsRequired)]
        public DateTime DateShip { get; set; }
    }
}
