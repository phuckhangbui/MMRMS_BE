using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MachineCheckRequest
{
    public class CreateMachineCheckRequestDto
    {
        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.MachineSerialNumberRequired)]
        public string ContractId { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.NoteRequired)]
        public string Note { get; set; }
    }
}
