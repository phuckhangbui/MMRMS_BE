using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MachineSerialNumber
{
    public class MachineSerialNumberCreateRequestDto
    {
        [Required(ErrorMessage = MessageConstant.MachineSerialNumber.SerialNumberRequired)]
        public string SerialNumber { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.MachineSerialNumber.MachineIdRequired)]
        public int MachineId { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineSerialNumber.ForceWhenNoComponentInMachineRequired)]
        public bool ForceWhenNoComponentInMachine { get; set; }
    }
}
