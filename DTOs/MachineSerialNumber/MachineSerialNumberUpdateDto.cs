using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MachineSerialNumber
{
    public class MachineSerialNumberUpdateDto
    {
        [Required(ErrorMessage = MessageConstant.MachineSerialNumber.ActualRentPriceRequired)]
        public double ActualRentPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineSerialNumber.RentTimeCounterequired)]
        public int RentDaysCounter { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineSerialNumber.StatusRequired)]
        public string? Status { get; set; }
    }
}
