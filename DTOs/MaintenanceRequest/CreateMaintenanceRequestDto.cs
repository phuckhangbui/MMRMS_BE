using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MachineCheckRequest
{
    public class CreateMachineCheckRequestDto
    {
        [Required(ErrorMessage = MessageConstant.MaintanningTicket.ProductSerialNumberRequired)]
        public string ContractId { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.NoteRequired)]
        public string Note { get; set; }
    }
}
