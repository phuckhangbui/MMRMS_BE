using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MachineComponentStatus
{
    public class MachineSerialNumberUpdateDto
    {
        [Required(ErrorMessage = MessageConstant.MachineSerialNumber.ActualRentPriceRequired)]
        public double ActualRentPrice { get; set; }
        [Required(ErrorMessage = MessageConstant.MachineSerialNumber.RentTimeCounterequired)]
        public int RentDaysCounter { get; set; }

    }
}
