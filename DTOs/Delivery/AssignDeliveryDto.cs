using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Delivery
{
    public class AssignDeliveryDto
    {
        [Required(ErrorMessage = MessageConstant.Delivery.DeliveryIdRequired)]
        public int DeliveryId { get; set; }

        [Required(ErrorMessage = MessageConstant.Delivery.StaffIdRequired)]
        public int StaffId { get; set; }

        [Required(ErrorMessage = MessageConstant.Delivery.DateshipIsRequired)]
        public DateTime DateShip { get; set; }
    }
}
