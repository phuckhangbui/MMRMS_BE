using System.ComponentModel.DataAnnotations;

namespace DTOs.MachineSerialNumber
{
    public class MachineSerialNumberRentRequestDto
    {
        [Required]
        public int MachineId { get; set; }

        [Required]
        public string SerialNumber { get; set; }
    }
}
